import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";
import SportList from "../components/select-lists/sport-list.vue";
import ScheduleYearList from "../components/select-lists/schedule-year-list.vue";
import vueSelectBase from "../components/base/vue-selectbase/vue-selectbase.vue";

var teamCustomCodeImporter = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "sport-list": SportList,
        "schedule-year-list": ScheduleYearList,
        "vue-selectbase": vueSelectBase,
    },

    data: {
        fileMsg: "",
        loading: false,
        fileList: null,
        uploadFile: null,
        selectedFile: "",
        fileValid: false,
        selectedSport: null,
        selectedSportName: "",
        selectedScheduleYear: null,
        selectedScheduleYearName: "",
        sheetNames: [],
        selectedSheet: "",
        userFileData: null,
        colMap: {},
        mapApproved: false,
        teamsVerified: false,
        processComplete: false,
        processSuccess: false,
        mapOptions: [
            { id: "Unmapped", name: "Unmapped" },
            { id: "TeamId", name: "Team Id" },
            { id: "Team", name: "Team Name" },
            { id: "Section", name: "Section" },
            { id: "Group", name: "Group" },
            { id: "Conference", name: "Conference" },
            { id: "Division", name: "Division" },
            { id: "GroupExchange", name: "Group Exchange" },
        ],
        teams: [],
    },

    created() {
        var vm = this;
        // load any files that were uploaded recently
        $.get("/siteapi/teams/customcodeimporterfilelist", (data) => { // success
            if (data !== undefined && data !== null && data.length > 0) {
                vm.fileList = data;
            }
        }, "json");
    },

    mounted() {
    },

    computed: {
        requireValidateFile() {
            return this.selectedSport === null || this.selectedSport <= 0 ||
                this.selectedScheduleYear === null || this.selectedScheduleYear <= 0 ||
                this.selectedFile === null || this.selectedFile === '';
        },
        requireFile() {
            return this.requireValidateFile || !this.fileValid;
        },

        requireMap() {
            return !this.requireFile && (this.colMap === null || !this.mapApproved);
        },

        requireVerify() {
            return !this.requireFile && !this.requireMap && !this.teamsVerified && !this.processComplete;
        },

        uploadFileInfo() {
            if (this.uploadFile !== null) {
                return this.$refs.file.value;
            } else {
                return "Choose file to upload...";
            }
        },

        userFileDataPreview() {
            if (this.userFileData !== undefined && this.userFileData !== null && this.userFileData.length > 0) {
                if (this.userFileData.length > 20) {
                    return this.userFileData.slice(0, 19);
                }
                return this.userFileData;
            }
            return [];
        },

        mapRequirementsMet() {
            let mapHasTeams = false;
            if (this.mapRequirementsHasTeam) {
                mapHasTeams = this.mappedTeams.length > 0;
            }
            return this.mapRequirementsHasTeam && this.mapRequirementsHasMap && mapHasTeams;
        },

        mapRequirementsHasTeam() {
            if (this.userFileData === undefined || this.userFileData === null || this.userFileData.length <= 0) {
                return false;
            }

            let mapped = false;

            let refRow = this.userFileData[0];
            let propIdx = 0;
            for (var prop in refRow) {
                let propColMap = this.colMap[propIdx];
                if (propColMap.MapType === "Team" || propColMap.MapType === "TeamId") {
                    mapped = true;
                    break;
                }

                propIdx += 1;
            }
            return mapped;
        },

        mapRequirementsHasMap() {
            if (this.userFileData === undefined || this.userFileData === null || this.userFileData.length <= 0) {
                return false;
            }
            let mapped = false;

            let refRow = this.userFileData[0];
            let propIdx = 0;
            for (var prop in refRow) {
                let propColMap = this.colMap[propIdx];
                if (propColMap.MapType !== "Team" && propColMap.MapType !== "TeamId" && propColMap.MapType !== "Unmapped") {
                    mapped = true;
                    break;
                }

                propIdx += 1;
            }
            return mapped;
        },

        mapRequirementsMessages() {
            let messages = [];

            // 1. Check to make sure that either team id or team name mapping is supplied
            if (!this.mapRequirementsHasTeam) {
                messages.push("You must define the mapping value for either Team Id or Team.");
            }

            // 2. Check to make sure that at least one non-Unmapped, not team id / team column is defined
            if (!this.mapRequirementsHasMap) {
                messages.push("You must define at least one column that contains values to apply to custom codes");
            }

            // 3. Only allow to move forward if there is at least 1 mapped team found
            if (this.mapRequirementsHasTeam && this.mappedTeams.length === 0) {
                messages.push("The matching process was unable to map any teams. This may be because teams have not been provisioned for the selected sport and schedule year, or because the mapping data is incorrect.");
            }

            return messages;
        },

        mappedTeams() {
            let mapped = [];

            if (this.userFileData === undefined || this.userFileData === null || this.userFileData.length <= 0) {
                return mapped;
            }

            let colIdx = 0;
            let teamMapCol = "";
            let teamMapType = "";
            for (var col in this.userFileData[0]) {
                let map = this.colMap[colIdx];
                if (map.MapType === "Team" || map.MapType === "TeamId") {
                    teamMapCol = map.ColName;
                    teamMapType = map.MapType;
                }
                colIdx += 1;
            }

            if (teamMapCol === "" || teamMapType === "") {
                return mapped;
            }

            for (var i = 0; i < this.userFileData.length; i++) {
                let userFileRow = this.userFileData[i];
                let selTeam = null;
                switch (teamMapType) {
                    case "TeamId":
                        selTeam = this.teams.find(t => t.id == userFileRow[teamMapCol]);
                        break;
                    case "Team":
                        selTeam = this.teams.find(t => t.name === userFileRow[teamMapCol]);
                        break;
                }

                if (selTeam !== undefined && selTeam !== null) {
                    mapped.push({"MappedTeamId": selTeam.id, "RowData": this.userFileData[i]});
                }
            }

            return mapped;
        },

        unmappedTeams() {
            let unmapped = [];

            let mapped = [];

            if (this.userFileData === undefined || this.userFileData === null || this.userFileData.length <= 0) {
                return mapped;
            }

            let colIdx = 0;
            let teamMapCol = "";
            let teamMapType = "";
            for (var col in this.userFileData[0]) {
                let map = this.colMap[colIdx];
                if (map.MapType === "Team" || map.MapType === "TeamId") {
                    teamMapCol = map.ColName;
                    teamMapType = map.MapType;
                }
                colIdx += 1;
            }

            if (teamMapCol === "" || teamMapType === "") {
                return mapped;
            }

            for (var i = 0; i < this.userFileData.length; i++) {
                let userFileRow = this.userFileData[i];
                let selTeam = null;
                switch (teamMapType) {
                    case "TeamId":
                        selTeam = this.teams.find(t => t.id == userFileRow[teamMapCol]);
                        break;
                    case "Team":
                        selTeam = this.teams.find(t => t.name === userFileRow[teamMapCol]);
                        break;
                }

                if (selTeam === undefined || selTeam === null) {
                    unmapped.push(this.userFileData[i]);
                }
            }

            return unmapped;
        },

        savedRequest() {
            let request = {
                "SportId": this.selectedSport,
                "ScheduleYearId": this.selectedScheduleYear,
                "Codes": [],
            }

            if (this.mappedTeams.length <= 0) {
                return request;
            }

            for (var i = 0; i < this.mappedTeams.length; i++) {
                let teamRow = this.mappedTeams[i];
                let teamId = teamRow.MappedTeamId;
                let teamRowData = teamRow.RowData;

                for (var col in this.colMap) {
                    let mapInfo = this.colMap[col];
                    if (mapInfo.MapType !== "Unmapped" && mapInfo.MapType !== "TeamId" && mapInfo.MapType !== "Team") {
                        // this is a mapped column that has team custom code in it!
                        let code = { "CustomCodeId": 0, "CodeName": mapInfo.MapType, "CodeValue": teamRowData[mapInfo.ColName], "TeamId": teamId};
                        request.Codes.push(code);
                    }
                }
            }

            return request;
        }
    },

    methods: {
        setSport(ev) {
            if (ev !== undefined && ev !== null && ev.id) {
                this.selectedSport = ev.id;
                this.selectedSportName = ev.name;
                return;
            }
            this.selectedSport = null;
            this.selectedSportName = "";
        },
        setScheduleYear(ev) {
            if (ev !== undefined && ev !== null && ev.id) {
                this.selectedScheduleYear = ev.id;
                this.selectedScheduleYearName = ev.label;
                return;
            }
            this.selectedScheduleYear = null;
            this.selectedScheduleYearName = "";
        },
        setExistingFile(ev) {
            if (ev !== undefined && ev !== null && ev.id) {
                this.selectedFile = ev.id;
                return;
            }
            this.selectedFile = "";
        },
        setUploadFile() {
            this.uploadFile = this.$refs.file.files[0];
        },

        doValidateFile() {
            if (this.uploadFile !== null) {
                this.doValidateUploadFile();
            } else {
                this.doValidateChooseFile();
            }
        },

        doValidateUploadFile() {
            // need to upload the file the user has selected, and then read back the contents as a json stream for our mapping step
            var vm = this;

            vm.loading = true;

            var data = new FormData();
            data.append("file", vm.uploadFile);
            data.append("sportName", vm.selectedSportName);
            data.append("scheduleYearName", vm.selectedScheduleYearName);

            $.ajax({
                url: "/siteapi/teams/customcodeimporterupload",
                type: "POST",
                data: data,
                processData: false,
                contentType: false,
                enctype: 'multipart/form-data',
                success: function (data) {
                    if (data !== undefined && data !== null) {
                        if (data.success) {
                            vm.selectedFile = data.obj;
                            vm.fileValid = true;
                            vm.loading = false;

                            vm.doGetSheetNames();
                        } else {
                            vm.fileMsg = data.message;
                            vm.loading = false;
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    vm.loading = false;
                    vm.fileMsg = "There was a problem processing your file upload";
                }
            });
        },

        doValidateChooseFile() {
            // need to get the file the user selected back as a json stream for our mapping step

            this.fileValid = true;
            this.doGetSheetNames();
            this.doGetTeams();
        },

        doGetSheetNames() {
            var vm = this;
            vm.loading = true;
            $.get("/siteapi/teams/customcodeimportersheetnames?filename=" + this.selectedFile, (data) => { // success
                if (data !== undefined && data !== null && data.length > 0) {
                    vm.sheetNames = data;
                    vm.loading = false;
                }
            }, "json");
        },

        doGetTeams() {
            var vm = this;
            $.get("/siteapi/teams/corecoveragelist?sportid=" + this.selectedSport + "&scheduleyearid=" + this.selectedScheduleYear, (data) => { // success
                if (data !== undefined && data !== null && data.length > 0) {
                    vm.teams = data;
                }
            }, "json");
        },

        setWorksheet(ev) {
            if (ev !== undefined && ev !== null && ev.id) {
                this.selectedSheet = ev.id;

                // now load in the preview data
                this.doGetSheetData();
                return;
            }
            this.selectedSheet = "";
            this.userFileData = null;
        },

        doGetSheetData() {
            var vm = this;

            vm.loading = true;

            $.get("/siteapi/teams/customcodeimportersheetdata?filename=" + this.selectedFile + "&sheetname=" + this.selectedSheet, (data) => { // success
                if (data !== undefined && data !== null && data.length > 0) {
                    vm.userFileData = data;

                    // try to init the colMap data using matches found in the sheet header                    
                    if (vm.userFileData.length > 0) {
                        var idx = 0;
                        for (var prop in vm.userFileData[0]) {
                            var mapData = { "ColName": prop, "MapType": "Unmapped" };
                            var mapOpt = this.mapOptions.find(m => m.name.toLowerCase() === prop.toLowerCase());
                            if (mapOpt !== undefined && mapOpt !== null) {
                                mapData.MapType = mapOpt.id;
                            }

                            this.$set(this.colMap, idx, mapData);

                            idx += 1;
                        }
                    }

                    vm.loading = false;
                }
            }, "json");
        },

        getSelMapOpt(val) {
            let opt = this.mapOptions.find(mo => mo.id === val);
            return opt;
        },

        setMap(colIdx, colName, mapSel) {
            let mapType = "";
            if (mapSel !== undefined && mapSel !== null && mapSel.id) {
                mapType = mapSel.id;
            }

            let mapData = {
                "ColName": colName,
                "MapType": mapType
            };

            if (this.colMap[colIdx] !== undefined) {
                this.colMap[colIdx].ColName = colName;
                this.colMap[colIdx].MapType = mapType;
            } else {
                this.$set(this.colMap, colIdx, mapData);
            }
        },

        doMapFile() {
            this.mapApproved = true;
        },

        saveCustomCodes() {
            var vm = this;
            let requestData = vm.savedRequest;
            vm.loading = true;

            $.ajax({
                method: "POST",
                url: "/siteapi/teams/customcodeimportersave",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(requestData),
                dataType: "json"
            })
                .done(function (data) {
                    vm.processComplete = true;
                    if (data !== undefined && data !== null && data.success) {
                        vm.processSuccess = true;
                    }
                    vm.loading = false;
                }, 'json');
        }
    }
});