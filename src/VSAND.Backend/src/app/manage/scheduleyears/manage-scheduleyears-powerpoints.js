import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import InputCalendar from "../../components/base/input-calendar/input-calendar.vue";
import InputDateTime from "../../components/base/input-datetime/input-datetime.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var powerPointsConfig = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "input-calendar": InputCalendar,
        "input-datetime": InputDateTime,
        "widget-wrapper": WidgetWrapper
    },
    data: {
        powerPoints: window.powerPoints
    },
    methods: {
        updatePowerPoints() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: '/SiteApi/ScheduleYears/PowerPoints/' + powerPoints.ppConfigId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(powerPoints),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        powerPoints.ppConfigId = data.id;

                        self.$bvToast.toast('You have updated the Power Points configuration', {
                            title: 'Updated Power Points',
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
        }
    }
});