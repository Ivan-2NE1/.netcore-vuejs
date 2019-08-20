import InputField from "../components/base/input-field/input-field.vue";
import TableGrid from "../components/base/table-grid/table-grid.vue";
import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var teamCustomCodes = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "table-grid": TableGrid,
        "widget-wrapper": WidgetWrapper
    },
    data: {
        team: window.team,
        customCodes: window.customCodes,
        addCustomCodeForm: {},
        rowColInfo: window.colInfo
    },
    created() {
        this.addCustomCodeForm = this.getAddCustomCodeForm();
    },
    computed: {
        sortedCustomCodes() {
            return this.customCodes.sort(function (a, b) {
                var aUpper = a.codeName.toUpperCase();
                var bUpper = b.codeName.toUpperCase();

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
        createCustomCode() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Teams/CustomCodes/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addCustomCodeForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newCustomCode = JSON.parse(JSON.stringify(self.addCustomCodeForm));
                        newCustomCode.customCodeId = data.id;
                        self.customCodes.push(newCustomCode);

                        self.$bvToast.toast('You have added ' + self.addCustomCodeForm.codeName, {
                            title: 'Added Custom Code',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addCustomCodeForm = self.getAddCustomCodeForm();
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
        saveEditCustomCode(customCode) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/SiteApi/Teams/CustomCodes/' + customCode.customCodeId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(customCode),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.customCodes = self.customCodes.map(function (s) {
                            // update the customCode if the Id matches
                            if (s.customCodeId === customCode.customCodeId) {
                                return customCode;
                            }

                            // leave it unchanged
                            return s;
                        });

                        self.$bvToast.toast('You have updated ' + customCode.codeName, {
                            title: 'Updated Custom Code',
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
        deleteCustomCode(customCode) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Teams/CustomCodes/' + customCode.customCodeId,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.customCodes = self.customCodes.filter(function (s) {
                            return s.customCodeId !== customCode.customCodeId;
                        });

                        self.$bvToast.toast('You have deleted ' + customCode.codeName, {
                            title: 'Deleted Custom Code',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The custom code was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddCustomCodeForm() {
            return {
                codeName: "",
                codeValue: "",
                teamId: this.team.teamId
            };
        }
    }
});