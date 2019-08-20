import WidgetWrapper from "../../../components/base/widget-wrapper/widget-wrapper.vue";
import TableGrid from "../../../components/base/table-grid/table-grid.vue";
import InputField from "../../../components/base/input-field/input-field.vue";
import vueSelectBase from "../../../components/base/vue-selectbase/vue-selectbase.vue";

var sportSchedules = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "vue-selectbase": vueSelectBase,
        "table-grid": TableGrid,
    },

    data: {
        fileMsg: "",
        loading: false,
        fileList: null,
        uploadFile: null,
        selectedSport: { id: window.sportId, name: "" },
        selectedSportName: window.sportName,
        selectedScheduleYear: { id: window.scheduleYearId, name: ""},
        selectedScheduleYearName: window.scheduleYearName,
        scheduleFiles: window.scheduleFiles,
        fileImportStatus: {
            current: 0,
            total: 0
        },
    },

    created() {
        
    },

    mounted() {
    },

    computed: {
        uploadFileInfo() {
            if (this.uploadFile !== null) {
                return this.$refs.file.value;
            } else {
                return "Choose file to upload...";
            }
        },
    },

    methods: {
        setUploadFile() {
            this.uploadFile = this.$refs.file.files[0];
        },
        
        processFile(file) {
            window.location = "/Manage/ScheduleYears/" + file.scheduleYearId + "/Schedules/" + file.sportId + "/ProcessFile/" + file.fileId;
        },
        
        uploadScheduleFile() {
            if (this.uploadFile === undefined || this.uploadFile === null) {
                this.fileMsg = "Choose a file to upload!";
                return;
            }

            // need to upload the file the user has selected, and then read back the contents as a json stream for our mapping step
            var vm = this;

            vm.loading = true;

            var data = new FormData();
            data.append("file", vm.uploadFile);
            data.append("sportId", vm.selectedSport.id);
            data.append("sportName", vm.selectedSportName);
            data.append("scheduleYearId", vm.selectedScheduleYear.id);
            data.append("scheduleYearName", vm.selectedScheduleYearName);

            $.ajax({
                url: "/SiteApi/ScheduleYears/ScheduleFileUpload",
                type: "POST",
                data: data,
                processData: false,
                contentType: false,
                enctype: 'multipart/form-data',
                success: function (data) {
                    if (data !== undefined && data !== null) {
                        if (data.success) {
                            vm.scheduleFiles.push(data.obj);
                            vm.uploadFile = null;
                            vm.loading = false;
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
    }
});