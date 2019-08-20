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
        states: [],
        addStateForm: {},
        rowColInfo: window.colInfo
    },
    created() {
        var self = this;

        $.get('/siteapi/states', function (data) {
            self.states = data;
        }, 'json');

        this.addStateForm = this.getAddStateForm();
    },
    computed: {
        sortedStates() {
            return this.states.sort(function (a, b) {
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
        createState() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/siteapi/states/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addStateForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newState = JSON.parse(JSON.stringify(self.addStateForm));
                        newState.stateId = data.id;
                        self.states.push(newState);

                        self.$bvToast.toast('You have added ' + self.addStateForm.name, {
                            title: 'Added State',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addStateForm = self.getAddStateForm();
                    } else {
                        self.$bvToast.toast(data.message, {
                            title: 'An Error Occurred',
                            appendToast: true,
                            solid: true,
                            noAutoHide: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },
        saveEditState(state) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/siteapi/states/' + state.stateId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(state),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.states = self.states.map(function (s) {
                            // update the state if the Id matches
                            if (s.stateId === state.stateId) {
                                return state;
                            }

                            // leave it unchanged
                            return s;
                        });

                        self.$bvToast.toast('You have updated ' + state.name, {
                            title: 'Updated State',
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
        deleteState(state) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/siteapi/states/' + state.stateId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(state),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.states = self.states.filter(function (s) {
                            return s.stateId !== state.stateId;
                        });

                        self.$bvToast.toast('You have deleted ' + state.name, {
                            title: 'Deleted State',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The state was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddStateForm() {
            return {
                name: "",
                abbreviation: "",
                pubAbbreviation: ""
            };
        }
    }
});