using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Arc.Data;
using VSAND.Arc.ViewModels.Arc.Slim;

namespace VSAND.Arc.Util
{
    public static class Kinesis
    {
        private static string _accessKeyId = "AKIASYIBXYIGDA2C2AFR";
        private static string _secretAccessKey = "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";

        private static string _roleArn = "arn:aws:iam::397853141546:role/kinesis-advancelocal-external";
        private static string _roleSessionName = "High_School_Sports_NJAM_Dev";

        private static string _streamName = "com.arcpublishing.advancelocal.contentv2.ans.v3";
        private static Amazon.RegionEndpoint _region = Amazon.RegionEndpoint.USEast1;

        public static List<string> _publications = new List<string> { "nj", "masslive", "lehighvalleylive", "syracuse" };

        public static async Task UpdateStoriesFromStream(ArcContentContext context)
        {
            var credentials = await GetCredentialsAsync();

            var client = new AmazonKinesisClient(credentials, _region);
            var stream = await client.DescribeStreamAsync(new DescribeStreamRequest { StreamName = _streamName });

            var shards = stream.StreamDescription.Shards;
            foreach (var shard in shards)
            {
                if (shard.ShardId != "shardId-000000000001")
                {
                    continue;
                }

                var shardIterator = await client.GetShardIteratorAsync(new GetShardIteratorRequest
                {
                    ShardId = shard.ShardId,
                    StreamName = _streamName,
                    ShardIteratorType = ShardIteratorType.AT_TIMESTAMP,
                    Timestamp = (await GetLastTimeStampLocal(context)).ToLocalTime()
                });

                var shardIteratorToken = shardIterator.ShardIterator;

                while (shardIteratorToken != null)
                {
                    var records = new List<string>();

                    // avoid hitting AWS rate limit
                    await Task.Delay(1000);

                    var recordsResponse = await client.GetRecordsAsync(new GetRecordsRequest { ShardIterator = shardIteratorToken });

                    foreach (var record in recordsResponse.Records)
                    {
                        var data = Gzip.Unzip(record.Data.ToArray());
                        if (data.StartsWith("https"))
                        {
                            var httpClient = new RestClient();
                            var request = new RestRequest(data);

                            var resource = await httpClient.ExecuteTaskAsync(request);
                            if (resource.IsSuccessful)
                            {
                                var decompressedResource = Gzip.Unzip(resource.RawBytes);

                                records.Add(decompressedResource);
                            }
                        }
                        else
                        {
                            records.Add(data);
                        }
                    }

                    await WriteStream(context, records);

                    shardIteratorToken = recordsResponse.NextShardIterator;
                }
            }
        }

        private static async Task WriteStream(ArcContentContext context, List<string> records)
        {
            foreach (var sRecord in records)
            {
                var recordViewModel = JsonConvert.DeserializeObject<SlimContentOperation>(sRecord);

                var publication = recordViewModel.Body.CanonicalWebsite;
                if (!_publications.Contains(publication))
                {
                    continue;
                }

                if (!recordViewModel.Body.Websites.ContainsKey(publication) || recordViewModel.Body.Websites[publication].WebsiteSection?.Id != "/highschoolsports")
                {
                    continue;
                }

                var contentRecord = recordViewModel.ToEntityModel(sRecord);

                var dbRecord = await context.ContentOperation.FirstOrDefaultAsync(co => co.ContentOperationId == recordViewModel.Id);

                // if we don't have the record, always create it regardless of status
                if (dbRecord == null)
                {
                    contentRecord.CreatedDateUtc = DateTime.UtcNow;
                    contentRecord.ModifiedDateUtc = DateTime.UtcNow;

                    context.ContentOperation.Add(contentRecord);
                }
                else
                {
                    context.Entry(dbRecord).State = EntityState.Detached;

                    contentRecord.CreatedDateUtc = dbRecord.CreatedDateUtc;
                    contentRecord.ModifiedDateUtc = DateTime.UtcNow;

                    if (dbRecord.Published)
                    {
                        if (contentRecord.Published)
                        {
                            context.Update(contentRecord);
                        }
                        else
                        {
                            // Ignore this. We don't want it until it is published again
                        }
                    }
                    else
                    {
                        context.Update(contentRecord);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task<DateTime> GetLastTimeStampLocal(ArcContentContext context)
        {
            // if there are no records found, go back seven days and load the data
            var oRet = DateTime.UtcNow.AddDays(-7);

            var lastModifiedRecord = await context.ContentOperation.OrderByDescending(co => co.LastOperationDateUtc).FirstOrDefaultAsync();
            if (lastModifiedRecord != null)
            {
                oRet = lastModifiedRecord.LastOperationDateUtc;
            }

            return oRet;
        }

        private static async Task<Credentials> GetCredentialsAsync()
        {
            var securityClient = new AmazonSecurityTokenServiceClient(_accessKeyId, _secretAccessKey, _region);

            var roleRequest = new AssumeRoleRequest
            {
                RoleArn = _roleArn,
                RoleSessionName = _roleSessionName
            };

            var roleResponse = await securityClient.AssumeRoleAsync(roleRequest);

            return roleResponse.Credentials;
        }
    }
}
