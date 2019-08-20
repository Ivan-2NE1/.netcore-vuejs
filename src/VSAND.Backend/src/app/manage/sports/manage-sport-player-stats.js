import InputField from "../../components/base/input-field/input-field.vue";
import InputSelect from "../../components/base/input-select/input-select.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import SportSidebar from "../../components/navs/sport-sidebar.vue";

import draggable from 'vuedraggable';

import { ToastPlugin, ModalPlugin, ButtonPlugin } from "bootstrap-vue";

Vue.use(ToastPlugin);
Vue.use(ModalPlugin);
Vue.use(ButtonPlugin);

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
        this.addPlayerStatForm = this.getAddPlayerStatForm();
        this.movePlayerStatForm = this.getPlayerStatMoveForm();
    },
    data: {
        sport: window.sport,
        playerStatCategoryId: window.playerStatCategoryId,
        addPlayerStatForm: {},
        movePlayerStatForm: {},
        editRow: null,
        blah: false,
        dataTypeOptions: [
            "System.Boolean",
            "System.Decimal",
            "System.Int32",
            "System.Integer",
            "System.String",
            "VSAND.BattingSingles",
            "VSAND.BattingTotalHits",
            "VSAND.BowlingHighScore",
            "VSAND.Distance",
            "VSAND.FaceOffsWinningPct",
            "VSAND.GamesPlayed",
            "VSAND.Height",
            "VSAND.InningsPitched",
            "VSAND.LongTime",
            "VSAND.ShortDistance",
            "VSAND.SmallDistance",
            "VSAND.SmallHeight",
            "VSAND.SmallTime",
            "VSAND.SprintTime",
            "VSAND.TennisMatchRecord",
            "VSAND.Time"
        ]
    },
    computed: {
        playerStatCategory() {
            return this.sport.playerStatCategories.find(psc => psc.sportPlayerStatCategoryId === this.playerStatCategoryId);
        },
        playerStatCategoryName() {
            return this.playerStatCategory.name;
        }
    },
    methods: {
        getAddPlayerStatForm() {
            return {
                name: "",
                abbreviation: "",
                dataType: "",
                calculated: false,
                enabled: false,
                sportId: this.sport.sportId,
                sportPlayerStatCategoryId: this.playerStatCategoryId
            };
        },
        addPlayerStat() {
            var self = this;

            self.addPlayerStatForm.sortOrder = self.playerStatCategory.playerStats.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/PlayerStats/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addPlayerStatForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newPlayerStat = JSON.parse(JSON.stringify(self.addPlayerStatForm));
                        newPlayerStat.sportPlayerStatId = data.id;
                        sport.playerStatCategories.find(psc => psc.sportPlayerStatCategoryId === self.playerStatCategoryId).playerStats.push(newPlayerStat);

                        self.$bvToast.toast('You have added ' + self.addPlayerStatForm.name, {
                            title: 'Added Player Stat',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addPlayerStatForm = self.getAddPlayerStatForm();
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
        deletePlayerStat(playerStat) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Sports/' + playerStat.sportId + "/PlayerStats/" + playerStat.sportPlayerStatId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(playerStat),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        var playerStatCategory = self.sport.playerStatCategories.find(psc => psc.sportPlayerStatCategoryId === self.playerStatCategoryId);

                        playerStatCategory.playerStats = playerStatCategory.playerStats.filter(function (s) {
                            return s.sportPlayerStatId !== playerStat.sportPlayerStatId;
                        });

                        self.$bvToast.toast('You have deleted ' + playerStat.name, {
                            title: 'Deleted Player Stat',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Player Stat was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        saveMetaOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/PlayerStats/SortOrder/Update/" + self.playerStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.playerStatCategory.playerStats),
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
        startEditPlayerStat(playerStat) {
            this.editRow = JSON.parse(JSON.stringify(playerStat));
        },
        savePlayerStat() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.editRow.sportId + "/PlayerStats/" + self.editRow.sportPlayerStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Player Stat Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        var playerStatCategory = self.sport.playerStatCategories.find(psc => psc.sportPlayerStatCategoryId === self.playerStatCategoryId);

                        playerStatCategory.playerStats = playerStatCategory.playerStats.map(ps => {
                            if (ps.sportPlayerStatId === self.editRow.sportPlayerStatId) {
                                return self.editRow;
                            }

                            return ps;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The player stat was not saved.', {
                            title: 'Player Stat Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getPlayerStatMoveForm() {
            return {
                playerStatId: null,
                playerStatCategoryId: null
            };
        },
        moveStat() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/PlayerStats/Update/Move?playerStatId=" + self.movePlayerStatForm.playerStatId + "&playerStatCategoryId=" + self.movePlayerStatForm.playerStatCategoryId,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === true) {
                        self.$bvToast.toast('Player Stat moved successfully.', {
                            title: 'Success',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        var playerStatCategory = self.sport.playerStatCategories.find(psc => psc.sportPlayerStatCategoryId === self.playerStatCategoryId);

                        playerStatCategory.playerStats = playerStatCategory.playerStats.filter(ps => {
                            return ps.sportPlayerStatId !== self.movePlayerStatForm.playerStatId;
                        });

                        self.movePlayerStatForm = self.getPlayerStatMoveForm();
                    } else {
                        self.$bvToast.toast('An error occurred. The player stat was not saved.', {
                            title: 'Player Stat Save Error',
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