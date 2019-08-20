
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import ScheduleYearList from "../../components/select-lists/schedule-year-list.vue";
import SportList from "../../components/select-lists/sport-list.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var manageSchool = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "schedule-year-list": ScheduleYearList,
        "sport-list": SportList
    },
    data: {
        school: window.school,
        scheduleYear: {},
        sports: null,
        sport: null
    },
    created() {
        this.loadSportsList();
    },
    computed: {
        teams() {
            var self = this;
            return school.teams.filter(t => t.scheduleYearId === this.scheduleYear.id).sort(function (a, b) {
                var aUpper = self.getSportName(a.sportId).toUpperCase();
                var bUpper = self.getSportName(b.sportId).toUpperCase();

                if (aUpper < bUpper) {
                    return -1;
                }
                if (aUpper > bUpper) {
                    return 1;
                }
                return 0;
            });
        }
    },
    methods: {
        loadSportsList() {
            var self = this;
            $.get("/SiteApi/Sports", function (data) {
                self.sports = data;
            }, 'json');
        },
        getSportName(sportId) {
            if (this.sports === null) {
                return "";
            }

            var sport = this.sports.find(s => s.sportId === sportId);
            return (sport !== undefined) ? sport.name : "";
        },
        addTeam() {
            var self = this;

            var requestData = {
                scheduleYearId: this.scheduleYear.id,
                sportId: this.sport.id,
                schoolId: this.school.schoolId
            };

            $.post("/SiteApi/Schools/Teams/Add", requestData, function (result) {
                if (result !== 0) {
                    self.$bvToast.toast('You have added ' + self.sport.name, {
                        title: 'Added Custom Code',
                        autoHideDelay: 5000,
                        appendToast: true,
                        solid: true,
                        variant: "success"
                    });

                    requestData.teamId = result;
                    self.school.teams.push(requestData);
                    self.sport = null;
                }
                else {
                    self.$bvToast.toast('The request form was invalid.', {
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