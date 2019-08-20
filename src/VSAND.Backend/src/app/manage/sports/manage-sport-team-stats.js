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
        this.addTeamStatForm = this.getAddTeamStatForm();
        this.moveTeamStatForm = this.getTeamStatMoveForm();
    },
    data: {
        sport: window.sport,
        teamStatCategoryId: window.teamStatCategoryId,
        addTeamStatForm: {},
        moveTeamStatForm: {},
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
        teamStatCategory() {
            return this.sport.teamStatCategories.find(psc => psc.sportTeamStatCategoryId === this.teamStatCategoryId);
        },
        teamStatCategoryName() {
            return this.teamStatCategory.name;
        }
    },
    methods: {
        getAddTeamStatForm() {
            return {
                name: "",
                abbreviation: "",
                dataType: "",
                calculated: false,
                enabled: false,
                sportId: this.sport.sportId,
                sportTeamStatCategoryId: this.teamStatCategoryId
            };
        },
        addTeamStat() {
            var self = this;

            self.addTeamStatForm.sortOrder = self.teamStatCategory.teamStats.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/TeamStats/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addTeamStatForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newTeamStat = JSON.parse(JSON.stringify(self.addTeamStatForm));
                        newTeamStat.sportTeamStatId = data.id;
                        sport.teamStatCategories.find(psc => psc.sportTeamStatCategoryId === self.teamStatCategoryId).teamStats.push(newTeamStat);

                        self.$bvToast.toast('You have added ' + self.addTeamStatForm.name, {
                            title: 'Added Team Stat',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addTeamStatForm = self.getAddTeamStatForm();
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
        deleteTeamStat(teamStat) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Sports/' + teamStat.sportId + "/TeamStats/" + teamStat.sportTeamStatId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(teamStat),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        var teamStatCategory = self.sport.teamStatCategories.find(psc => psc.sportTeamStatCategoryId === self.teamStatCategoryId);

                        teamStatCategory.teamStats = teamStatCategory.teamStats.filter(function (s) {
                            return s.sportTeamStatId !== teamStat.sportTeamStatId;
                        });

                        self.$bvToast.toast('You have deleted ' + teamStat.name, {
                            title: 'Deleted Team Stat',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Team Stat was not deleted.', {
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
                url: "/SiteApi/Sports/" + self.sport.sportId + "/TeamStats/SortOrder/Update/" + self.teamStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.teamStatCategory.teamStats),
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
        startEditTeamStat(teamStat) {
            this.editRow = JSON.parse(JSON.stringify(teamStat));
        },
        saveTeamStat() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.editRow.sportId + "/TeamStats/" + self.editRow.sportTeamStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Team Stat Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        var teamStatCategory = self.sport.teamStatCategories.find(psc => psc.sportTeamStatCategoryId === self.teamStatCategoryId);

                        teamStatCategory.teamStats = teamStatCategory.teamStats.map(ps => {
                            if (ps.sportTeamStatId === self.editRow.sportTeamStatId) {
                                return self.editRow;
                            }

                            return ps;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The team stat was not saved.', {
                            title: 'Team Stat Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getTeamStatMoveForm() {
            return {
                teamStatId: null,
                teamStatCategoryId: null
            };
        },
        moveStat() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/TeamStats/Update/Move?teamStatId=" + self.moveTeamStatForm.teamStatId + "&teamStatCategoryId=" + self.moveTeamStatForm.teamStatCategoryId,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === true) {
                        self.$bvToast.toast('Team Stat moved successfully.', {
                            title: 'Success',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        var teamStatCategory = self.sport.teamStatCategories.find(psc => psc.sportTeamStatCategoryId === self.teamStatCategoryId);

                        teamStatCategory.teamStats = teamStatCategory.teamStats.filter(ps => {
                            return ps.sportTeamStatId !== self.moveTeamStatForm.teamStatId;
                        });

                        self.moveTeamStatForm = self.getTeamStatMoveForm();
                    } else {
                        self.$bvToast.toast('An error occurred. The team stat was not saved.', {
                            title: 'Team Stat Save Error',
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