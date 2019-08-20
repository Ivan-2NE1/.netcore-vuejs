import InputField from "../../../components/base/input-field/input-field.vue";
import InputCalendar from "../../../components/base/input-calendar/input-calendar.vue";
import InputSelect from "../../../components/base/input-select/input-select.vue";
import WidgetWrapper from "../../../components/base/widget-wrapper/widget-wrapper.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var eventTypeConfig = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-calendar": InputCalendar,
        "input-select": InputSelect,
        "widget-wrapper": WidgetWrapper
    },
    data: {
        eventType: window.eventType,
        scoreboardTypeOptions: [{ value: 1, label: 'By County' }, { value: 2, label: 'By Section / Group' }, { value: 3, label: 'By Event Type' }],
        filterTypeOptions: ['School', 'Team', 'Self Advancing', 'Rounds']
    },
    methods: {
        updateEventType() {
            var self = this;
            var eventType = JSON.parse(JSON.stringify(self.eventType));

            $.ajax({
                method: "PUT",
                url: '/SiteApi/EventTypes/' + eventType.eventTypeId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(eventType),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have updated ' + eventType.name, {
                            title: 'Updated Event Type',
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