
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../components/base/input-field/input-field.vue";
import SportList from "../../components/select-lists/sport-list.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var manageSchool = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "sport-list": SportList
    },
    data: {
        school: window.school,
        sports: null,
        addCustomCodeForm: {},
        editRow: null
    },
    created() {
        this.loadSportsList();
        this.addCustomCodeForm = this.getAddCustomCodeForm();
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
        getAddCustomCodeForm() {
            return {
                codeName: "",
                codeValue: "",
                schoolId: this.school.schoolId,
                sportId: null
            };
        },
        startEditCustomCode(customCode) {
            this.editRow = JSON.parse(JSON.stringify(customCode));
        },
        addCustomCode() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Schools/" + self.school.schoolId + "/CustomCodes",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addCustomCodeForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data && data.success) {
                        var newCustomCode = JSON.parse(JSON.stringify(self.addCustomCodeForm));
                        newCustomCode.customCodeId = data.id;
                        self.school.customCodes.push(newCustomCode);

                        self.$bvToast.toast('You have added ' + self.addCustomCodeForm.codeName, {
                            title: 'Added Custom Code',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addCustomCodeForm = self.getAddCustomCodeForm();
                    } else {
                        self.$bvToast.toast(data ? data.message : 'The request object was invalid.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },
        deleteCustomCode(customCode) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Schools/' + self.school.schoolId + "/CustomCodes/" + customCode.customCodeId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(customCode),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.school.customCodes = self.school.customCodes.filter(function (s) {
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
                        self.$bvToast.toast('The Custom Code was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        saveCustomCode() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Schools/" + self.school.schoolId + "/CustomCodes/" + self.editRow.customCodeId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Custom Code Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.school.customCodes = self.school.customCodes.map(e => {
                            if (e.customCodeId === self.editRow.customCodeId) {
                                return self.editRow;
                            }

                            return e;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The custom code was not saved.', {
                            title: 'Custom Code Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        }
    }
});