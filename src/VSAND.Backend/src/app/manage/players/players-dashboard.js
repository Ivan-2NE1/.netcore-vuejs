
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../../components/base/input-field/input-field.vue";
import SchoolAutocomplete from "../../components/select-lists/school-autocomplete.vue";
import GraduationYear from "../../components/select-lists/graduationyear-list.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var playerDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "school-autocomplete": SchoolAutocomplete,
        "graduationyear-list": GraduationYear,
    },

    data: {
        showSearchForm: false,
        searchFirstName: window.searchFirstName,
        searchLastName: window.searchLastName,
        searchGraduationYear: (window.searchGraduationYear === null || window.searchGraduationYear === "" || window.searchGraduationYear === 0) ? null : window.searchGraduationYear.toString(),
        searchSchool: (window.searchSchool === null || window.searchSchool === "" || window.searchSchool === 0) ? null : window.searchSchool,
        mode: 'view',
        addPlayerForm: {}
    },
    created() {
        this.addPlayerForm = this.getAddPlayerForm();
    },

    methods: {
        doSearch() {            
            window.location = "/players?" + this.getSearchQuery(1);
        },

        gotopage(pageNumber) {
            window.location = "/players?" + this.getSearchQuery(pageNumber);
        },

        getSearchQuery(pageNumber) {
            var params = {
                f: this.searchFirstName,
                l: this.searchLastName,
                gy: this.searchGraduationYear,
                s: (this.searchSchool !== null && this.searchSchool !== "") ? this.searchSchool.id : 0,
                pg: pageNumber
            };
            return $.param(params);
        },

        getAddPlayerForm() {
            return {
                firstName: "",
                lastName: "",
                graduationYear: "",
                schoolId: null,
                height: "",
                weight: "",
                birthDate: ""
            };
        },

        createPlayer() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Players/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addPlayerForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        self.$bvToast.toast('You have added ' + self.addPlayerForm.firstName + " " + self.addPlayerForm.lastName, {
                            title: 'Added Player',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addPlayerForm = self.getAddPlayerForm();
                    } else {
                        self.$bvToast.toast(data ? data.message : 'The request body is invalid.', {
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
