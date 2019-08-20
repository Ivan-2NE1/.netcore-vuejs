import InputField from "../../components/base/input-field/input-field.vue";
import InputSelect from "../../components/base/input-select/input-select.vue";
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
        "input-select": InputSelect,
        "input-checkbox": InputCheckbox,
        "widget-wrapper": WidgetWrapper,
        "sport-sidebar": SportSidebar,
        draggable
    },
    created() {
        this.addEventForm = this.getAddEventForm();
    },
    data: {
        sport: window.sport,
        addEventForm: {},
        editRow: null,
        resultTypes: [
            { label: 'Individual Result', value: 'Individual' },
            { label: '4-Player Result', value: '4Player' },
            { label: 'Player v. Player', value: 'PlayerVPlayer' },
            { label: '2-Players v. 2-Players', value: '2PlayersV2Players' },
            { label: 'Custom', value: 'Custom' }
        ]
    },
    methods: {
        addEvent() {
            var self = this;

            self.addEventForm.defaultSort = self.sport.sportEvents.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Sports/" + sport.sportId + "/Events",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addEventForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data && data.success) {
                        var newEvent = JSON.parse(JSON.stringify(self.addEventForm));
                        newEvent.sportEventId = data.id;
                        sport.sportEvents.push(newEvent);

                        self.$bvToast.toast('You have added ' + self.addEventForm.name, {
                            title: 'Added Event',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addEventForm = self.getAddEventForm();
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
        deleteEvent(event) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Sports/' + event.sportId + "/Events/" + event.sportEventId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(event),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.sport.sportEvents = self.sport.sportEvents.filter(function (s) {
                            return s.sportEventId !== event.sportEventId;
                        });

                        self.$bvToast.toast('You have deleted ' + event.name, {
                            title: 'Deleted Event',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Event was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddEventForm() {
            return {
                enabled: false,
                name: "",
                abbreviation: "",
                resultType: "",
                resultHandler: "",
                defaultEnabled: false,
                maxResults: "0",
                sportId: self.sport.sportId
            };
        },
        saveEventOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/Events/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.sport.sportEvents),
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
        startEditEvent(event) {
            this.editRow = JSON.parse(JSON.stringify(event));
        },
        saveEvent() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.editRow.sportId + "/Events/" + self.editRow.sportEventId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Event Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.sport.sportEvents = self.sport.sportEvents.map(e => {
                            if (e.sportEventId === self.editRow.sportEventId) {
                                return self.editRow;
                            }

                            return e;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The event was not saved.', {
                            title: 'Event Save Error',
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