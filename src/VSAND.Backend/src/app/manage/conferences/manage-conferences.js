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
        conferences: [],
        addConferenceForm: {},
        rowColInfo: window.colInfo
    },
    created() {
        var self = this;

        $.get('/siteapi/conferences', function (data) {
            self.conferences = data;
        }, 'json');

        this.addConferenceForm = this.getAddConferenceForm();
    },
    computed: {
        sortedConferences() {
            return this.conferences.sort(function (a, b) {
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
        createConference() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/siteapi/conferences/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addConferenceForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newConference = JSON.parse(JSON.stringify(self.addConferenceForm));
                        newConference.conferenceId = data.id;
                        self.conferences.push(newConference);

                        self.$bvToast.toast('You have added ' + self.addConferenceForm.name, {
                            title: 'Added Conference',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addConferenceForm = self.getAddConferenceForm();
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
        saveEditConference(conference) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/siteapi/conferences/' + conference.conferenceId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(conference),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.conferences = self.conferences.map(function (s) {
                            // update the conference if the Id matches
                            if (s.conferenceId === conference.conferenceId) {
                                return conference;
                            }

                            // leave it unchanged
                            return s;
                        });

                        self.$bvToast.toast('You have updated ' + conference.name, {
                            title: 'Updated Conference',
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
        deleteConference(conference) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/siteapi/conferences/' + conference.conferenceId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(conference),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.conferences = self.conferences.filter(function (s) {
                            return s.conferenceId !== conference.conferenceId;
                        });

                        self.$bvToast.toast('You have deleted ' + conference.name, {
                            title: 'Deleted Conference',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The conference was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddConferenceForm() {
            return {
                name: ""
            };
        }
    }
});