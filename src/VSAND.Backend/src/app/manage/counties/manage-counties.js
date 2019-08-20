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
        counties: [],
        addCountyForm: {},
        rowColInfo: window.colInfo
    },
    created() {
        var self = this;

        $.get('/siteapi/counties', function (data) {
            self.counties = data;
        }, 'json');

        this.addCountyForm = this.getAddCountyForm();
    },
    computed: {
        sortedCounties() {
            return this.counties.sort(function (a, b) {
                var aUpper = a.name.toUpperCase();
                var bUpper = b.name.toUpperCase();

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
        createCounty() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/siteapi/counties/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addCountyForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newCounty = JSON.parse(JSON.stringify(self.addCountyForm));
                        newCounty.countyId = data.id;
                        self.counties.push(newCounty);

                        self.$bvToast.toast('You have added ' + self.addCountyForm.name, {
                            title: 'Added County',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addCountyForm = self.getAddCountyForm();
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
        saveEditCounty(county) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/siteapi/counties/' + county.countyId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(county),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.counties = self.counties.map(function (s) {
                            // update the county if the Id matches
                            if (s.countyId === county.countyId) {
                                return county;
                            }

                            // leave it unchanged
                            return s;
                        });

                        self.$bvToast.toast('You have updated ' + county.name, {
                            title: 'Updated County',
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
        deleteCounty(county) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/siteapi/counties/' + county.countyId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(county),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.counties = self.counties.filter(function (s) {
                            return s.countyId !== county.countyId;
                        });

                        self.$bvToast.toast('You have deleted ' + county.name, {
                            title: 'Deleted County',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The county was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddCountyForm() {
            return {
                name: "",
                countyAbbr: "",
                state: ""
            };
        }
    }
});