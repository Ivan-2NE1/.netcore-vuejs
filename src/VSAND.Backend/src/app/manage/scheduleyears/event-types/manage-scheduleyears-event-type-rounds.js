import InputField from "../../../components/base/input-field/input-field.vue";
import InputCalendar from "../../../components/base/input-calendar/input-calendar.vue";
import WidgetWrapper from "../../../components/base/widget-wrapper/widget-wrapper.vue";

import draggable from 'vuedraggable';

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var eventTypeConfig = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-calendar": InputCalendar,
        "widget-wrapper": WidgetWrapper,
        draggable
    },
    data: {
        rounds: window.rounds,
        eventTypeId: window.eventTypeId,
        editRow: null,
        addRoundForm: null
    },
    created() {
        this.addRoundForm = this.getAddRoundForm();
    },
    methods: {
        addRound() {
            var self = this;

            self.addRoundForm.sortOrder = self.rounds.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/EventTypes/" + self.eventTypeId + "/Rounds",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addRoundForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newRound = JSON.parse(JSON.stringify(self.addRoundForm));
                        newRound.roundId = data.id;
                        rounds.push(newRound);

                        self.$bvToast.toast('You have added ' + self.addRoundForm.name, {
                            title: 'Added Round',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addRoundForm = self.getAddRoundForm();
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
        deleteRound(round) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/EventTypes/' + round.eventTypeId + "/Rounds/" + round.roundId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(round),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.rounds = self.rounds.filter(function (s) {
                            return s.roundId !== round.roundId;
                        });

                        self.$bvToast.toast('You have deleted ' + round.name, {
                            title: 'Deleted Round',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Round was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        updateRound() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/EventTypes/" + self.editRow.eventTypeId + "/Rounds/" + self.editRow.roundId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Round Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.rounds = self.rounds.map(r => {
                            if (r.roundId === self.editRow.roundId) {
                                return self.editRow;
                            }

                            return r;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The round was not saved.', {
                            title: 'Round Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        saveRoundOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/EventTypes/" + self.eventTypeId + "/Rounds/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.rounds),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === true) {
                        self.$bvToast.toast('You have successfully saved the sort order.', {
                            title: 'Sort Order Updated',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('An error occurred. The sort order was not saved. Please refresh and try again.', {
                            title: 'Sort Order Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        startEditRound(round) {
            this.editRow = JSON.parse(JSON.stringify(round));
        },
        getAddRoundForm() {
            return {
                eventTypeId: this.eventTypeId,
                name: "",
                startDate: null,
                endDate: null
            };
        }
    }
});