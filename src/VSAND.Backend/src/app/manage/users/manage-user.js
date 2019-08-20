import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import PublicationList from "../../components/select-lists/publication-list.vue";
import SchoolList from "../../components/select-lists/school-autocomplete.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var gameDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "publication-list": PublicationList,
        "school-autocomplete": SchoolList
    },

    data: {
        newPassword: "",
        user: window.user,
        userRoleCategories: window.userRoleCategories
    },

    computed: {
        userRoles() {
            return this.user.appxUser.appxUserRoles.map(r => r.roleId);
        },

        primaryRoleCat() {
            return this.userRoleCategories.find(rc => rc.roleCat === "UserFunction");
        },

        otherRoleCats() {
            return this.userRoleCategories.filter(rc => rc.roleCat !== "UserFunction");
        },

        masterAccountRole() {
            return this.primaryRoleCat.roles.find(c => c.roleName === "SchoolMasterAccount");
        }
    },

    methods: {
        isInRole(roleId) {
            return this.userRoles.includes(roleId);
        },

        roleChecked(enabled, role) {
            if (enabled === true) {
                user.appxUser.appxUserRoles.push(role);
            } else {
                user.appxUser.appxUserRoles = user.appxUser.appxUserRoles.filter(r => r.roleId !== role.roleId);
            }
        },

        updateUser() {
            var self = this;
            var requestData = {
                email: this.user.email,
                username: this.user.userName,
                password: this.newPassword,
                firstName: this.user.appxUser.firstName,
                lastName: this.user.appxUser.lastName,
                mobilePhone: this.user.phoneNumber,
                otherPhone: this.user.appxUser.phoneNumber,
                isAdmin: this.user.appxUser.isAdmin,
                userRoles: this.userRoles,
                publicationId: this.user.appxUser.publicationId,
                schoolId: this.user.appxUser.schoolId
            };

            $.ajax({
                method: "PUT",
                url: '/SiteApi/Users/' + this.user.id,
                data: requestData,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have updated ' + requestData.firstName + ' ' + requestData.lastName, {
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
