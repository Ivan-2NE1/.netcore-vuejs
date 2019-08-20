import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var gameDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField
    },

    data: {
        newPassword: "",
        user: window.user
    },

    methods: {
        updateUser() {
            var self = this;
            var requestData = {
                email: this.user.email,
                username: this.user.userName,
                password: this.newPassword,
                firstName: this.user.appxUser.firstName,
                lastName: this.user.appxUser.lastName,
                mobilePhone: this.user.phoneNumber,
                otherPhone: this.user.appxUser.phoneNumber
            };

            $.ajax({
                method: "PUT",
                url: '/SiteApi/Account/' + this.user.id,
                data: requestData,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('Your account has been updated.', {
                            title: 'Updated User',
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
        }
    }
});
