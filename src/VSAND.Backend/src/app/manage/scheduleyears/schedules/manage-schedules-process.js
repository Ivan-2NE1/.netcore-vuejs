import WidgetWrapper from "../../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../../components/base/input-checkbox/input-checkbox.vue";
import vueSelectBase from "../../../components/base/vue-selectbase/vue-selectbase.vue";
import SchoolAutocomplete from "../../../components/select-lists/school-autocomplete.vue";
import StateList from "../../../components/select-lists/state-list.vue";
import CountyList from "../../../components/select-lists/county-list.vue";

var signalR = require("@aspnet/signalr");

import { ToastPlugin, ProgressPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);
Vue.use(ProgressPlugin);

var sportScheduleProcess = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "vue-selectbase": vueSelectBase,
        "school-autocomplete": SchoolAutocomplete,
        "state-list": StateList,
        "input-checkbox": InputCheckbox,
        "county-list": CountyList,
    },

    data: {
        fileId: window.fileId,
        scheduleItems: window.scheduleItems,
        resolveOptions: [
            { id: "Selected", name: "Selected Recommendation" },
            { id: "Create", name: "Create New School" },
            { id: "Search", name: "Search Result" },
            { id: "Ignore", name: "Do not import"}
        ],
        loading: false,
        autoResolved: false,
        inBatch: false,
        showCount: 10,
        importSubmitted: false,
        importSubmitProblem: false,
        importJob: {
            current: 0,
            total: 0
        },
        commitSubmitted: false,
        commitSubmitProblem: false,
        commitErrorMessage: "",
        commitJob: {
            current: 0,
            total: 0
        }
    },

    created() {
        var vm = this;
        this.connection = new signalR.HubConnectionBuilder().withUrl("/SchedulingHub").build();

        this.connection.on("importStatus", data => {
            vm.importSubmitted = true;
            vm.importJob.current = data.current;
            vm.importJob.total = data.total;
        });

        this.connection.on("commitStatus", data => {
            vm.commitSubmitted = true;
            vm.commitJob.current = data.current;
            vm.commitJob.total = data.total;
        });

        this.connection.on("abortMessage", data => {
            vm.commitErrorMessage = data;
            vm.commitSubmitted = false;
            vm.commitJob.current = 0;
            vm.commitJob.total = 0;
        });

        this.connection.onclose(() => {
            vm.startSignalRConnection();
        });

        this.startSignalRConnection();
    },

    mounted() {
    },

    computed: {
        unresolvedList() {
            let unresolved = [];
            if (this.autoResolved) {
                unresolved = this.scheduleItems.filter(r => r.schoolId === null && !r.resolveMethodAccept && r.resolveToSchoolId !== null && r.resolveMethod === "Selected");
            } else {
                unresolved = this.scheduleItems.filter(r => r.schoolId === null && r.name !== null && r.name !== "" && !r.resolveMethodAccept)                
            }            
            unresolved.sort((a, b) => (a.name.toLowerCase() > b.name.toLowerCase()) ? 1 : -1);
            if (!this.autoResolved) {
                unresolved = unresolved.slice(0, this.showCount);
            }
            return unresolved;
        },

        unresolvedCount() {
            return this.scheduleItems.filter(r => r.schoolId === null && !r.resolveMethodAccept).length;   
        }
    },

    methods: {
        startSignalRConnection() {
            var roomName = "fileId=" + window.fileId;

            var vm = this;
            this.connection.start()
                .then(() => {
                    vm.connection.invoke("JoinRoom", roomName);
                })
                .catch(function (err) {
                    setTimeout(() => vm.startSignalRConnection(), 1000);
                });
        },

        importRows() {
            var vm = this;

            vm.loading = true;
            $.get("/SiteApi/ScheduleYears/ScheduleFileImport/" + window.fileId, function (response) {
                if (response !== null && response.success === true) {
                    // it was successfully submitted
                    vm.importSubmitted = true;
                    vm.loading = false;
                } else {
                    // there was a problem submitting the request
                    vm.loading = false;
                }
            });
        },

        commitSchedule() {
            var vm = this;

            vm.loading = true;
            $.get("/SiteApi/ScheduleYears/ScheduleFileCommit/" + window.fileId, function (response) {
                if (response !== null && response.success === true) {
                    // it was successfully submitted
                    vm.commitSubmitted = true;
                    vm.loading = false;
                } else {
                    // there was a problem submitting the request
                    vm.loading = false;
                }
            });
        },

        refreshAfterImport() {
            window.location.reload(true);
        },

        getOpponentList(problem) {
            return problem.opponents.join(", ");
        },

        notYetResolved(problem) {
            switch (problem.resolveMethod) {
                case "Selected":
                case "Search":
                    return problem.resolveToSchoolId === undefined || problem.resolveToSchoolId === null || problem.resolvedToSchoolId <= 0;
                    break;
                case "Create":
                    return problem.renameTo === undefined || problem.renameTo === null || problem.renameTo === "" || problem.state === undefined || problem.state === null || problem.state === "";
                case "Ignore":
                    return false;
            }
            return true;
        },

        // Dropdown recommendation resolver
        setResolveToAutoSchool(problem, e) {
            if (e !== undefined && e !== null && e.id) {
                problem.resolveToSchoolId = e.id;
            } else {
                problem.resolveToSchoolId = null;
            }
        },

        // Search resolver
        setResolveToSchool(problem, e) {
            if (e !== undefined && e !== null && e.id) {
                problem.resolveToSchoolId = e.id;
            } else {
                problem.resolveToSchoolId = null;
            }
        },

        // Resolve creates new school
        setCreateSchoolName(problem, e) {
            console.log("setCreateSchoolName", e);
            if (e !== undefined && e !== null) {
                problem.renameTo = e.trim();
            }
        },
        setCreateSchoolState(problem, e) {
            if (e !== undefined && e !== null && e.id) {
                problem.state = e.id;
            } else {
                problem.state = "";
            }
        },
        setCreateSchoolCity(problem, e) {
            console.log("setCreateSchoolCity", e);
            if (e !== undefined && e !== null) {
                problem.city = e.trim();
            } else {
                problem.city = "";
            }
        },
        setCreateSchoolCounty(problem, e) {
            if (e !== undefined && e !== null && e.id) {
                problem.countyId = e.id;
            } else {
                problem.countyId = 0;
            }
        },


        getDefaultResolveToSelection(problem) {
            let found = problem.resolveToChoices.find(c => c.id === problem.resolveToSchoolId);
            return found;
        },

        getDefaultResolveMethodSelection(problem) {
            let found = this.resolveOptions.find(r => r.id === problem.resolveMethod);
            return found;
        },

        setResolveMethod(problem, e) {
            if (e !== undefined && e !== null && e.id) {
                problem.resolveMethod = e.id;

                if (problem.resolveMethod === "Ignore") {
                    problem.resolveToSchoolId = -1;
                }
            } else {
                problem.resolveMethod = "";
            }
        },

        //processAllAutoResolved() {
        //    var self = this;
        //    self.inBatch = true;
        //    self.loading = true;
        //    let oResolved = self.scheduleItems.filter(r => r.schoolId === null && !r.resolveMethodAccept && r.resolveToSchoolId !== null && r.resolveMethod === "Selected");
        //    oResolved.sort((a, b) => (a.name > b.name) ? 1 : -1);
        //    for (let i = 0; i < oResolved.length; i++) {
        //        let cur = oResolved[i];
        //        let problem = self.scheduleItems.filter(r => r.name === cur.name);
        //        self.acceptResolution(problem);
        //    }
        //    self.inBatch = false;
        //    self.loading = false;
        //},

        acceptResolution(problem) {
            // do an ajax call to the server to save the school id, get the team id on the related records
            // so that if this is closed and re-opened, the data is saved
            var self = this;
            self.loading = true;
            Vue.set(problem, "processing", true);

            var request = {
                name: problem.name,
                schoolId: problem.resolveMethod === "Create" ? 0 : problem.resolveToSchoolId,
                renameTo: problem.renameTo,
                state: problem.state,
                city: problem.city,
                privateSchool: problem.privateSchool,
                countyId: problem.countyId,
                skipCreation: problem.resolveMethod === "Ignore"
            }
            
            $.ajax({
                method: "POST",
                url: "/SiteApi/ScheduleYears/ScheduleFileResolve/" + self.fileId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(request),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        problem.schoolId = request.schoolId;
                        problem.resolveMethodAccept = true;
                        if (!self.inBatch) {
                            self.loading = false;
                        }
                        self.$bvToast.toast('You have resolved the schedule import row for ' + request.name, {
                            title: 'Resolved Schedule Import Row',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        Vue.set(problem, "processing", false);
                        if (!self.inBatch) {
                            self.loading = false;
                        }
                        self.$bvToast.toast(data.message, {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        }
    }
});