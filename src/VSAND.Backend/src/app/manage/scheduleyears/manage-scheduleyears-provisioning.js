import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputCalendar from "../../components/base/input-calendar/input-calendar.vue";

import { ToastPlugin, ProgressPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);
Vue.use(ProgressPlugin);

var signalR = require("@aspnet/signalr");

var scheduleYearSummary = new Vue({
    el: '#vueApp',
    components: {
        "widget-wrapper": WidgetWrapper,
        "input-calendar": InputCalendar
    },
    data: {
        scheduleYear: window.scheduleYear,
        sportId: window.sportId,
        rowColInfo: window.colInfo,
        availableSelected: [],
        selectedSelected: [],
        selectedSchools: [],
        provisioningJob: {
            current: 0,
            total: 0
        },
        addEventTypeForm: {}
    },
    created() {
        var vm = this;

        this.addEventTypeForm = this.getAddEventTypeForm();

        this.connection = new signalR.HubConnectionBuilder().withUrl("/ProvisioningHub").build();

        this.connection.on("status", data => {
            vm.provisioningJob.current = data.current;
            vm.provisioningJob.total = data.total;
        });

        this.connection.onclose(() => {
            vm.startSignalRConnection();
        });

        this.startSignalRConnection();
    },
    computed: {
        createdTeams() {
            return this.scheduleYear.provisioningSummary.filter(e => e.currentSeasonTeamId > 0);
        },
        availableSchools() {
            return this.scheduleYear.provisioningSummary.filter(e => e.currentSeasonTeamId === 0 && !this.selectedSchools.includes(e));
        },
        availableGamesOrRosters() {
            return this.availableSchools.filter(e => e.previousSeasonGameCount > 0 || e.previousSeasonRosterCount > 0);
        },
        availableGamesOrRostersCount() {
            return this.availableGamesOrRosters.length;
        },
        availableGames() {
            return this.availableSchools.filter(e => e.previousSeasonGameCount > 0);
        },
        availableGamesCount() {
            return this.availableGames.length;
        },
        availableRosters() {
            return this.availableSchools.filter(e => e.previousSeasonRosterCount > 0);
        },
        availableRostersCount() {
            return this.availableRosters.length;
        },
        availableInCoreCoverage() {
            return this.availableSchools.filter(e => e.coreCoverage);
        },
        availableInCoreCoverageCount() {
            return this.availableInCoreCoverage.length;
        },
        availableSelectedCount() {
            return this.availableSelected.length;
        }
    },
    methods: {
        getAddEventTypeForm() {
            return {
                sportId: this.sportId,
                scheduleYearId: this.scheduleYear.scheduleYearId,
                name: "Regular Season",
                venue: "",
                startDate: "",
                endDate: "",
                scoreboardTypeId: 1, // county
                defaultSelected: 1,
                sectionLabel: "",
                groupLabel: "",
                participatingTeams: null,
                filterType: null,
                filterName: ""
            };
        },
        pushSet(dataSet) {
            this.selectedSchools.push(...dataSet);
        },
        pushSelected() {
            this.selectedSchools.push(...this.availableSelected);
        },
        pushAll() {
            this.selectedSchools.push(...this.availableSchools);
        },
        pullSelected() {
            this.selectedSchools = this.selectedSchools.filter(e => !this.selectedSelected.includes(e));
        },
        pullAll() {
            this.selectedSchools = [];
        },
        startSignalRConnection() {
            var roomName = "scheduleYearId=" + window.scheduleYearId + "&sportId=" + window.sportId;

            var vm = this;
            this.connection.start()
                .then(() => {
                    vm.connection.invoke("JoinRoom", roomName);
                })
                .catch(function (err) {
                    setTimeout(() => vm.startSignalRConnection(), 1000);
                });
        },
        provisionTeams() {
            var requestData = {
                scheduleYearId: window.scheduleYearId,
                sportId: window.sportId,
                schoolIds: this.selectedSchools.map(function (school) {
                    return school.schoolId;
                })
            };

            $.ajax({
                method: "POST",
                url: "/Manage/ScheduleYears/ProvisionTeams",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(requestData),
                dataType: 'json'
            });
        },
        addEventType() {
            var self = this;
            var eventType = JSON.parse(JSON.stringify(self.addEventTypeForm));

            $.ajax({
                method: "POST",
                url: '/SiteApi/EventTypes/',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(eventType),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        window.location.reload();
                    } else {
                        self.$bvToast.toast(data.message, {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
    }
});