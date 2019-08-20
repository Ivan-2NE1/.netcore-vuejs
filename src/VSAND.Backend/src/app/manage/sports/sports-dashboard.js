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
        addSportForm: {}
    },
    created() {
        this.addSportForm = this.getAddSportForm();
    },
    computed: {

    },
    methods: {
        createSport() {
            var self = this;

            $.ajax({
                method: "POST",
                url: "/siteapi/sports/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addSportForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newSport = JSON.parse(JSON.stringify(self.addSportForm));
                        newSport.sportId = data.id;

                        window.location = "/sports/" + data.id + '/edit';

                        self.$bvToast.toast('You have added ' + self.addSportForm.name, {
                            title: 'Added Sport',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addSportForm = self.getAddSportForm();
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
        getAddSportForm() {
            return {
                name: "",
                abbreviation: "",
                season: "",
                enabled: false,
                meetName: "",
                meetType: "",
                enableLowScoreWins: false,
                allowTie: false,
                allowMultiEventPerDay: false,
                enableTriPlusScheduling: false,
                enableJerseyNumber: false,
                enablePosition: false,
                enableGamePosition: false,
                enableStarter: false,
                enableRosterOrder: false,
                gameRosterOrderLabel: "",
                countZeroValueStats: false,
                playerName: "",
                playerNamePlural: "",
                enablePeriodScoring: false,
                enableScoringPlayByPlay: false,
                enablePowerPoints: false,
                allowOT: false,
                allowShootout: false
            };
        }
    }
});