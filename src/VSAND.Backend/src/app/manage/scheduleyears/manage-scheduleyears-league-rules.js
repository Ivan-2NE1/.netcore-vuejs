import InputField from "../../components/base/input-field/input-field.vue";
import InputSelect from "../../components/base/input-select/input-select.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import VueSelectbase from "../../components/base/vue-selectbase/vue-selectbase.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var adminDashboard = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-select": InputSelect,
        "vue-selectbase": VueSelectbase,
        "widget-wrapper": WidgetWrapper
    },
    data: {
        scheduleYearId: window.scheduleYearId,
        sportId: window.sportId,
        leagueRules: window.leagueRules,
        leagueRuleOptions: window.leagueRuleOptions,
        ruleTypeOptions: [{ value: 'conference', label: 'Conference' }, { value: 'division', label: 'Division' }, { value: 'rule', label: 'Rule' }]
    },
    methods: {
        createLeagueRule() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/SiteApi/ScheduleYears/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addLeagueRuleForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newLeagueRule = JSON.parse(JSON.stringify(self.addLeagueRuleForm));
                        newLeagueRule.leagueRuleId = data.id;
                        self.leagueRules.push(newLeagueRule);

                        self.$bvToast.toast('You have added ' + self.addLeagueRuleForm.name, {
                            title: 'Added League Rule',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addLeagueRuleForm = self.getAddLeagueRuleForm();
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
        updateLeagueRule(leagueRule) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/SiteApi/ScheduleYears/' + self.scheduleYearId + '/LeagueRules/' + leagueRule.leagueRuleId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(leagueRule),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have updated ' + leagueRule.conference + ' - ' + leagueRule.division, {
                            title: 'Updated League Rule',
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
        populateFromExistingData() {
            var self = this;

            $.ajax({
                method: "POST",
                url: '/SiteApi/ScheduleYears/' + self.scheduleYearId + '/LeagueRules/' + self.sportId + '/PopulateFromExistingData',
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast(data.message + ' You will be redirect momentarily.', {
                            title: 'Populated League Rules',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        setTimeout(() => {
                            location.reload();
                        }, 5000);
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
        populateFromPreviousYear() {
            var self = this;

            $.ajax({
                method: "POST",
                url: '/SiteApi/ScheduleYears/' + self.scheduleYearId + '/LeagueRules/' + self.sportId + '/PopulateFromPreviousYear',
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast(data.message + ' You will be redirect momentarily.', {
                            title: 'Populated League Rules',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        setTimeout(() => {
                            location.reload();
                        }, 5000);
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
        simpleOptionsAdapter(item) {
            return {
                label: item.label,
                key: item.value,
                value: item.value
            };
        }
    }
});