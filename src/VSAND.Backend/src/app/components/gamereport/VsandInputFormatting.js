export default {
    methods: {
        getDataTypeInputMask(dataType) {
            let mask = null;
            switch (dataType.toLowerCase()) {
                case "vsand.height":
                    mask = {
                        mask: ['9{1,2}-99[ 9/9]', '9{1,2}-99[ 9/9]'], placeholder: "11-06 1/4"};
                case "vsand.smalltime":
                    mask = { mask: ['99:99', '99:99'], placeholder: "mm:ss" };
                    break;
                case "vsand.time":
                    mask = { mask: ['9:99.99', '9:99.99'], placeholder: "m:ss.nnn" };
                    break;
                case "vsand.sprinttime":
                    mask = { mask: ['99.99', '99.99'], placeholder: "ss.nn" };
                    break;
            }
            return mask;
        },

        maskDataTypeValue(rawValue, dataType) {
            let formattedValue = "";
            if (rawValue > 0) {
               
                // need to process this to a correctly formatted value based on the stat input type
                switch (dataType.toLowerCase()) {
                    case "vsand.smalltime":
                        var vDuration = moment.duration(rawValue / 10000); // there are 10K ticks in a millisecond
                        formattedValue = moment.utc(vDuration.as("milliseconds")).format("mm:ss");
                        break;
                    case "vsand.time":
                        var vDuration = moment.duration(rawValue / 10000); // there are 10K ticks in a millisecond
                        formattedValue = moment.utc(vDuration.as("milliseconds")).format("m:ss.SS");
                        break;
                    case "vsand.sprinttime":
                        var vDuration = moment.duration(rawValue / 10000); // there are 10K ticks in a millisecond
                        formattedValue = moment.utc(vDuration.as("milliseconds")).format("ss.SS");
                        break;
                }
            }
            return formattedValue;
        }
    },

};