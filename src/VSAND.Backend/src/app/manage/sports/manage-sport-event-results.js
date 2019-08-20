import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import SportSidebar from "../../components/navs/sport-sidebar.vue";

import draggable from 'vuedraggable';

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var adminDashboard = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "widget-wrapper": WidgetWrapper,
        "sport-sidebar": SportSidebar,
        draggable
    },
    created() {
        this.addEventResultForm = this.getAddEventResultForm();
    },
    data: {
        sport: window.sport,
        addEventResultForm: {},
        editRow: null
    },
    methods: {
        addEventResult() {
            var self = this;

            self.addEventResultForm.defaultSort = self.sport.eventResults.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Sports/" + sport.sportId + "/EventResults",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addEventResultForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data && data.success) {
                        var newEventResult = JSON.parse(JSON.stringify(self.addEventResultForm));
                        newEventResult.sportEventResultId = data.id;
                        sport.eventResults.push(newEventResult);

                        self.$bvToast.toast('You have added ' + self.addEventResultForm.name, {
                            title: 'Added Event Result',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addEventResultForm = self.getAddEventResultForm();
                    } else {
                        self.$bvToast.toast(data ? data.message : 'The request object was invalid.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },
        deleteEventResult(event) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Sports/' + event.sportId + "/EventResults/" + event.sportEventResultId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(event),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.sport.eventResults = self.sport.eventResults.filter(function (s) {
                            return s.sportEventResultId !== event.sportEventResultId;
                        });

                        self.$bvToast.toast('You have deleted ' + event.name, {
                            title: 'Deleted Event Result',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Event Result was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddEventResultForm() {
            return {
                name: "",
                isTie: false,
                sportId: self.sport.sportId
            };
        },
        saveEventResultOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/EventResults/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.sport.eventResults),
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
        startEditEventResult(event) {
            this.editRow = JSON.parse(JSON.stringify(event));
        },
        saveEventResult() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.editRow.sportId + "/EventResults/" + self.editRow.sportEventResultId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Event Result Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.sport.eventResults = self.sport.eventResults.map(e => {
                            if (e.sportEventResultId === self.editRow.sportEventResultId) {
                                return self.editRow;
                            }

                            return e;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The event result was not saved.', {
                            title: 'Event Result Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        resultTypeOptionAdapter(option) {
            return {
                label: option.label,
                key: option.value,
                value: option.value
            };
        }
    }
});