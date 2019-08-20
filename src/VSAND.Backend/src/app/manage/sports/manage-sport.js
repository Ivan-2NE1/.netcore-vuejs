import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import InputSelect from "../../components/base/input-select/input-select.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import SportSidebar from "../../components/navs/sport-sidebar.vue";
import SeasonList from "../../components/select-lists/season-list.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var adminDashboard = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "input-select": InputSelect,
        "season-list": SeasonList,
        "widget-wrapper": WidgetWrapper,
        "sport-sidebar": SportSidebar
    },
    data: {
        editSportForm: window.sport,
        positions: window.positions,
        playerStatCategories: window.playerStatCategories
    },
    computed: {

    },
    methods: {
        saveEditSport() {
            var self = this;

            var sport = JSON.parse(JSON.stringify(self.editSportForm));

            $.ajax({
                method: "PUT",
                url: '/siteapi/sports/' + sport.sportId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(sport),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have updated ' + sport.name, {
                            title: 'Updated Sport',
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

        playerStatCategoryOptionsAdapter(category) {
            return {
                label: category.name,
                key: "statCategory_" + category.sportPlayerStatCategoryId,
                value: category.sportPlayerStatCategoryId
            };
        },

        positionOptionsAdapter(position) {
            return {
                label: position.name,
                key: "position_" + position.sportPositionId,
                value: position.sportPositionId
            };
        }
    }
});