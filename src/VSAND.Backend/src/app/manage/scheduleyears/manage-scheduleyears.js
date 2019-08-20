import InputField from "../../components/base/input-field/input-field.vue";
import TableGrid from "../../components/base/table-grid/table-grid.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var adminDashboard = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "table-grid": TableGrid,
        "widget-wrapper": WidgetWrapper
    },
    data: {
        scheduleYears: [],
        addScheduleYearForm: {},
        rowColInfo: window.colInfo
    },
    created() {
        var self = this;

        $.get('/siteapi/scheduleyears/listall', function (data) {
            self.scheduleYears = data;
        }, 'json');

        this.addScheduleYearForm = this.getAddScheduleYearForm();
    },
    computed: {
        sortedScheduleYears() {
            if (this.scheduleYears === undefined || this.scheduleYears === null || this.scheduleYears.length === 0) return [];

            var oFirst = this.scheduleYears.find(function (x) {
                return x.active === true;
            });

            var oRest = this.scheduleYears.filter(function (x) {
                return x.active !== true;
            });

            // TODO: This should return the single active schedule year first, then all others by EndYear descending
            oRest.sort(function (a, b) {
                if (a.endYear > b.endYear) {
                    return -1;
                }
                if (a.endYear < b.endYear) {
                    return 1;
                }
                return 0;
            });

            var oRet = [];
            oRet.push(oFirst);
            oRet.push(...oRest);
            return oRet;
        }
    },
    methods: {
        createScheduleYear() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/siteapi/scheduleyears/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addScheduleYearForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newScheduleYear = JSON.parse(JSON.stringify(self.addScheduleYearForm));
                        newScheduleYear.scheduleYearId = data.id;
                        self.scheduleYears.push(newScheduleYear);

                        self.$bvToast.toast('You have added ' + self.addScheduleYearForm.name, {
                            title: 'Added Schedule Year',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addScheduleYearForm = self.getAddScheduleYearForm();
                    } else {
                        self.$bvToast.toast(data.message, {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },
        updateScheduleYear(scheduleYear) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/siteapi/scheduleyears/' + scheduleYear.scheduleYearId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(scheduleYear),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.scheduleYears = self.scheduleYears.map(function (s) {
                            // update the schedule year if the Id matches
                            if (s.scheduleYearId === scheduleYear.scheduleYearId) {
                                return scheduleYear;
                            }

                            // leave it unchanged
                            return s;
                        });

                        self.$bvToast.toast('You have updated ' + scheduleYear.name, {
                            title: 'Updated Schedule Year',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
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
        getAddScheduleYearForm() {
            return {
                name: "",
                endYear: "",
                active: false
            };
        },
        makeActiveScheduleYear(scheduleYearId) {
            var self = this;

            var request = {
                "": scheduleYearId
            }

            $.ajax({
                method: "GET",
                url: "/SiteApi/ScheduleYears/SetActive?scheduleYearId=" + scheduleYearId,
                //contentType: 'application/json; charset=utf-8',
                //data: JSON.stringify(request),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        let activeSyName = "";
                        for (let i = 0; i < self.scheduleYears.length; i++) {
                            var sy = self.scheduleYears[i];
                            if (sy.scheduleYearId !== scheduleYearId) {
                                sy.active = false;
                            } else {
                                sy.active = true;
                                activeSyName = sy.name;
                            }
                        }

                        self.$bvToast.toast('You have successfully updated the active schedule year to ' + activeSyName, {
                            title: 'Updated Active Schedule Year',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast(data.message, {
                            title: "Set Active Schedule Year Error",
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