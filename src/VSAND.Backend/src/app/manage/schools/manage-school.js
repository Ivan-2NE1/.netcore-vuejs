
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import StateList from "../../components/select-lists/state-list.vue";
import CountyList from "../../components/select-lists/county-list.vue";

import { ToastPlugin, ModalPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);
Vue.use(ModalPlugin);

var manageSchool = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "state-list": StateList,
        "county-list": CountyList
    },
    data: {
        school: window.school,
        masterAccountId: window.masterAccountId,
        masterAccountUsername: window.masterAccountUsername,
        masterAccountPassword: "",
        showDeleteModal: false
    },
    methods: {
        updateSchool() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/SiteApi/Schools/' + self.school.schoolId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.school),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have updated ' + school.name, {
                            title: 'Updated School',
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

        deleteSchool() {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Schools/' + school.schoolId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(school),
                dataType: 'json'
            })
                .done(function (data) {
                    self.showDeleteModal = false;

                    if (data.success === true) {
                        self.$bvToast.toast('You have deleted ' + school.name, {
                            title: 'Deleted School',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        setTimeout(() => {
                            window.location = "/schools";
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

        createMasterAccount() {
            var self = this;

            var requestData = {
                schoolId: this.school.schoolId,
                username: this.masterAccountUsername,
                password: this.masterAccountPassword
            };

            $.ajax({
                method: "POST",
                url: '/SiteApi/Users/SchoolMasterAccount',
                data: requestData,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have created a master account for ' + self.school.name, {
                            title: 'Master Account Created',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.masterAccountId = data.id;
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
        }
    }
});