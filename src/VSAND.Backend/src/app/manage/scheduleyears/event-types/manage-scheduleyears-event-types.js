import InputField from "../../../components/base/input-field/input-field.vue";
import InputCalendar from "../../../components/base/input-calendar/input-calendar.vue";
import InputSelect from "../../../components/base/input-select/input-select.vue";
import WidgetWrapper from "../../../components/base/widget-wrapper/widget-wrapper.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var eventTypesConfig = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-calendar": InputCalendar,
        "input-select": InputSelect,
        "widget-wrapper": WidgetWrapper
    },
    data: {
        eventTypes: window.eventTypes,
        scheduleYearId: window.scheduleYearId,
        sportId: window.sportId,
        addEventTypeForm: {},
        scoreboardTypeOptions: [{ value: 1, label: 'By County' }, { value: 2, label: 'By Section / Group' }, { value: 3, label: 'By Event Type' }],
        filterTypeOptions: [ 'School', 'Team', 'Self Advancing', 'Rounds' ]
    },
    created() {
        this.addEventTypeForm = this.getAddEventTypeForm();
    },
    methods: {
        getAddEventTypeForm() {
            return {
                sportId: this.sportId,
                scheduleYearId: this.scheduleYearId,
                name: "",
                venue: "",
                startDate: "",
                endDate: "",
                scoreboardType: null,
                sectionLabel: "",
                groupLabel: "",
                participatingTeams: null,
                filterType: null,
                filterName: ""
            };
        },
        addEventType() {
            var self = this;
            var eventType = JSON.parse(JSON.stringify(self.addEventTypeForm));

            $.ajax({
                method: "POST",
                url: '/SiteApi/EventTypes/',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(eventType),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        eventType.eventTypeId = data.id;

                        self.eventTypes.push(eventType);
                        self.addEventTypeForm = self.getAddEventTypeForm();

                        self.$bvToast.toast('You have added ' + eventType.name, {
                            title: 'Added Event Type',
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
        simpleOptionsAdapter(item) {
            return {
                label: item.label,
                key: item.value,
                value: item.value
            };
        }
    }
});