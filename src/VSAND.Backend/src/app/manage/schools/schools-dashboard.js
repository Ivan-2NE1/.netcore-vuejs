// Import any components that you need to use, and make sure to expose them in the components section, too!
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import StateList from "../../components/select-lists/state-list.vue";
import CountyList from "../../components/select-lists/county-list.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var gameDashboard = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "state-list": StateList,
        "county-list": CountyList
    },

    data: {
        // Reactive properties
        mode: 'view',
        showSearchForm: false,
        searchName: window.searchName,
        searchCity: window.searchCity,
        searchState: { id: window.searchState, name: window.searchState },
        searchCore: window.searchCore,
        schools: window.schools,
        addSchoolForm: {}
    },
    created() {
        // Load whatever we need to get via ajax (not included in the Model)
        // Setup any non-reactive properties here
        this.addSchoolForm = this.getAddSchoolForm();
    },

    mounted() {
        // Anything that needs to take place after the app is mounted
    },

    computed: {
        // Computed properties
    },

    watch: {
        // Data and Computed properties to watch
    },

    methods: {
        // Do stuff!

        doSearch() {
            var params = {
                name: this.searchName,
                city: this.searchCity,
                state: (this.searchState !== null) ? (this.searchState.id ? this.searchState.id : this.searchState) : "",
                core: this.searchCore,
                pg: 1
            };
            var query = $.param(params);
            window.location = "/schools?" + query;
        },

        gotopage(pageNumber) {
            var params = {
                name: this.searchName,
                city: this.searchCity,
                state: (this.searchState !== null && this.searchState !== "") ? this.searchState.id : "",
                core: this.searchCore,
                pg: pageNumber
            };
            var query = $.param(params);
            window.location = "/schools?" + query;
        },

        getAddSchoolForm() {
            return {
                name: "",
                state: null,
                countyId: null
            };
        },

        createSchool() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/siteapi/schools/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addSchoolForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        // var newSchool = JSON.parse(JSON.stringify(self.addSchoolForm));
                        // newSchool.schoolId = data.id;
                        // self.schools.push(newSchool);

                        self.$bvToast.toast('You have added ' + self.addSchoolForm.name, {
                            title: 'Added School',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        window.location = '/Schools/' + data.id;
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
        }
    }
});
