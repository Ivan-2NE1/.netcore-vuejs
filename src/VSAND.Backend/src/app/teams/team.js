import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";

import { ToastPlugin, ModalPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);
Vue.use(ModalPlugin);

var teamDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField
    },

    data: {
        team: window.team,
        filterOpponent: "",
        games: null,
        allowtie: false,
        teamrecord: [],
        fixRecordModal: false,
        oosInfo: null,
        headerBgVariant: 'dark',
        headerTextVariant: 'light',
        bodyBgVariant: 'light',
        bodyTextVariant: 'dark',
    },

    created() {
        // Load whatever we need to get via ajax (not included in the Model)
        // Setup any non-reactive properties here
        var vm = this;

        // load the sport for the game report from the API
        $.get("/siteapi/games/teamgames?teamid=" + vm.team.teamId, function (data) {
            vm.games = data;
            //if (data) {
            //    vm.allowtie = data[0].sport.allowTie;
            //}
            //var homescore = 0;
            //var opponentscore = 0;
            //var scoreelement = {};
            //var opponentname = null;
            //if (vm.games !== null) {
            //    for (var i = 0; i < vm.games.length; i++) {
            //        if (vm.games[i].teams[0].teamId == vm.team.teamId) {
            //            homescore = vm.games[i].teams[0].finalScore;
            //            opponentscore = vm.games[i].teams[1].finalScore;
            //            opponentname = vm.games[i].teams[1].name;
            //        }
            //        else if (vm.games[i].teams[1].teamId == vm.team.teamId) {
            //            homescore = vm.games[i].teams[1].finalScore;
            //            opponentscore = vm.games[i].teams[0].finalScore;
            //            opponentname = vm.games[i].teams[0].name;
            //        }
            //        var temp = homescore + "-" + opponentscore;
            //        scoreelement.date = vm.games[i].gameDate;
            //        scoreelement.result = temp;
            //        scoreelement.record = temp;
            //        scoreelement.opponent = opponentname;
            //        vm.scoreboard.push(scoreelement);
            //        scoreelement = {};
            //    }
            //}
        });

        //Get Team RecordInfo
        $.get("/siteapi/games/teamrecordinfo?teamid=" + vm.team.teamId, function (data) {
            vm.teamrecord = data[0];
        });
    },

    mounted() {
        // Anything that needs to happen after the DOM is ready
        var vm = this;
    },

    computed: {
        filteredSb() {
            var ret = this.games;
            if (this.filterOpponent !== null && this.filterOpponent !== "") {
                let filter = this.filterOpponent;
                // filter the games by the search string
                ret = this.games.filter(sb => {
                    return sb.opponent.includes(filter);
                });
            }

            return ret;
        }
    },

    methods: {
        setDeleted(id, value) {
            this.games.map( (game, index) => {
                if (game.gameReportId == id) {
                    this.games[index].deleted = value;
                }
            })
        },
        convertDate(dateTime) {
            var gamedate = new Date(dateTime);
            var dd = gamedate.getDate();
            var mm = gamedate.getMonth() + 1;
            var yyyy = gamedate.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var gamedate = mm + '/' + dd + '/' + yyyy;
            return gamedate;
        },
        
        deleteGameData(index) {
            var item = this.filteredSb[index];
            var vm = this;
            $.ajax({
                method: "DELETE",
                url: '/siteapi/games/deleteGameReport/' + item.gameReportId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(item),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        if (data.message == "False") {
                            vm.setDeleted(item.gameReportId, false);
                            vm.$bvToast.toast('You have successfully restored ' + item.gameReportId, {
                                title: 'Game Restored',
                                autoHideDelay: 5000,
                                appendToast: true,
                                variant: "success"
                            });

                        } else {
                            vm.setDeleted(item.gameReportId, true);
                            vm.$bvToast.toast("Deleted " + item.gameReportId + " successfully.", {
                                title: 'Game Deleted',
                                autoHideDelay: 5000,
                                appendToast: true,
                                variant: "success"
                            });
                        }

                    } else {
                        let action = "Deleting";
                        if (item.deleted) {
                            action = "Restoring";
                        }
                        vm.$bvToast.toast(data.message, {
                            title: "Problem " + action + " Game",
                            appendToast: true,
                            noAutoHide: true,
                            variant: "danger"
                        });
                    }
                });
        },

        showOosModal(gameReportId) {
            var item = this.games.find(g => g.gameReportId === gameReportId);

            if (item === undefined || item === null) {
                vm.$bvToast.toast("Could not find the item to load OOS record data for.", {
                    title: "Manage OOS Error",
                    appendToast: true,
                    noAutoHide: true,
                    variant: "danger"
                });
                return;
            }

            var vm = this;

            $.ajax({
                method: "GET",
                url: "/SiteApi/Teams/" + item.opponentId + "/OosRecord",
                cache: false
            }).done(function (data) {
                if (data !== undefined && data !== null) {
                    vm.oosInfo = data;
                    vm.fixRecordModal = true;
                } else {
                    vm.$bvToast.toast("Could not load the OOS record data for " + item.opponent, {
                        title: "Manage OOS Error",
                        appendToast: true,
                        noAutoHide: true,
                        variant: "danger"
                    });
                }
            });
        },

        updateOosRecord() {
            var vm = this;

            if (vm.oosInfo === undefined || vm.oosInfo === null || vm.oosInfo.teamId === null || vm.oosInfo.teamId <= 0) {
                // something is wrong in our data
                vm.$bvToast.toast("There was a problem getting the OOS info to save.", {
                    title: "Problem Saving OOS",
                    appendToast: true,
                    noAutoHide: true,
                    variant: "danger"
                });
                return;
            }

            let teamName = vm.oosInfo.teamName;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Teams/" + vm.oosInfo.teamId + "/OosRecord",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(vm.oosInfo),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {                        
                        vm.$bvToast.toast('The OOS record was updated for ' + teamName, {
                            title: 'OOS Record Updated',
                            autoHideDelay: 5000,
                            appendToast: true,
                            variant: "success"
                        });
                        vm.fixRecordModal = false;
                    } else {
                        vm.$bvToast.toast(data.message, {
                            title: 'OOS Record Error',
                            appendToast: true,
                            noAutoHide: true,
                            variant: "danger"
                        });
                    }
                });
        },
    }
});