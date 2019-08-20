import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import SchoolList from "../../components/select-lists/school-autocomplete.vue";
import PublicationList from "../../components/select-lists/publication-list.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var gameDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "school-autocomplete": SchoolList,
        "publication-list": PublicationList
    },

    data: {
        showSearchForm: false,
        searchForm: {
            username: window.searchUsername,
            email: window.searchEmail,
            first: window.searchFirstName,
            last: window.searchLastName,
            admin: window.searchIsAdmin,
            school: window.searchSchoolId,
            publication: window.searchPublicationId
        },
        addUserForm: {
            email: "",
            password: "",
            firstName: "",
            lastName: "",
            phone: ""
        },
        users: window.users,
        mode: 'view'
    },

    methods: {
        doSearch() {
            let params = this.searchForm;
            params.pg = 1;

            var query = $.param(params);
            window.location = "/Manage/Users?" + query;
        },

        gotopage(pageNumber) {
            let params = this.searchForm;
            params.pg = pageNumber;

            var query = $.param(params);
            window.location = "/Manage/Users?" + query;
        },

        createUser() {
            var self = this;
            var requestData = JSON.parse(JSON.stringify(this.addUserForm));

            $.post("/SiteApi/Users/", requestData, function (data) {
                if (data.success) {
                    self.$bvToast.toast('You have added ' + self.addUserForm.firstName + " " + self.addUserForm.lastName + ". You will be redirected momentarily.", {
                        title: 'Added User',
                        autoHideDelay: 5000,
                        appendToast: true,
                        solid: true,
                        variant: "success"
                    });

                    setTimeout(() => {
                        window.location = "/Manage/Users/" + data.id;
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
            }, "json");
        }
    }
});
