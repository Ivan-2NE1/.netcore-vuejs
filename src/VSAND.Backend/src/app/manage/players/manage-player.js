
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
        "graduationyear-list": GraduationYear
    },

    data: {
        player: window.player
    },

    methods: {
        updatePlayer() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Players/" + self.player.playerId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.player),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        self.$bvToast.toast('You have updated ' + self.player.firstName + " " + self.player.lastName, {
                            title: 'Updated Player',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
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
