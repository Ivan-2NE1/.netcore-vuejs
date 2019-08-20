<template>
    <div class="schedule card" v-if="scheduleData !== undefined && scheduleData !== null">
        <div class="card-body">
            <h1><strong>Schedule</strong> / Scoreboard</h1>
        </div>

        <div class="card-body border-top-0" v-if="!refTeam">
            <div class="list-group list-group-horizontal d-flex align-items-stretch">
                <a class="list-group-item list-group-item-action d-none d-md-block align-self-center"
                   v-on:click.prevent="setViewDate(scheduleData.prevViewStartDate)">
                    <i class="far fa fa-chevron-circle-left fa-2x"></i>
                </a>
                <template v-for="(dt,idx) in scheduleData.viewDates">
                    <a :class="['list-group-item', 'list-group-item-action ', 'd-md-block', ' align-self-center', {'active': activeDate.isSame(dt, 'day'), 'd-none': idx > 0 && idx < 3 || idx > 4}]"
                       v-on:click.prevent="setViewDate(dt)">
                        <span class="day-name">{{ getDayName(dt) }}</span><br />
                        <span class="day-date">{{ formatDisplayDate(dt) }}</span>
                    </a>
                </template>
                <a class="list-group-item list-group-item-action d-none d-md-block align-self-center"
                   v-on:click.prevent="setViewDate(scheduleData.nextViewStartDate)">
                    <i class="far fa fa-chevron-circle-right fa-2x"></i>
                </a>
                <v-date-picker v-model="viewDate"
                               mode="single"
                               :popover="{ placement: 'bottom', visibility: 'click' }"
                               v-on:input="setViewDate">
                    <a class="list-group-item list-group-item-action align-self-center"><i class="fal fa fa-calendar-alt fa-2x"></i></a>
                </v-date-picker>
            </div>
        </div>

        <div class="card-body p-0" v-if="scheduleData.games !== null && scheduleData.games.length > 0">
            <!-- Has Ref Display -->
            <table class="table table-striped table-borderless" v-if="refDisplay && (finalGames !== null && finalGames.length > 0 || scheduleGames !== null && scheduledGames.length > 0)">
                <thead v-if="finalGames !== null && finalGames.length > 0">
                    <tr>
                        <th v-if="refTeamId !== null && refTeamId > 0">Date</th>
                        <th v-if="selectedSportId === null || selectedSportId === 0">Sport</th>
                        <th>Opponent</th>
                        <th>Result</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="finalGames !== null && finalGames.length > 0">
                    <tr v-for="gr in finalGames" :key="gr.gameReportId">
                        <td v-if="refTeamId !== null && refTeamId > 0" v-once>{{ formatGameDate(gr.gameDate) }}</td>
                        <td v-if="selectedSportId === null || selectedSportId === 0">{{ gr.sport.name }}</td>
                        <td v-once>{{ formatOpponent(gr) }}</td>
                        <td v-once v-html="formatResult(gr)"></td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Game Details</a></td>
                    </tr>
                </tbody>
                <thead v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr>
                        <th v-if="refTeamId !== null && refTeamId > 0">Date</th>
                        <th v-if="selectedSportId === null || selectedSportId === 0">Sport</th>
                        <th>Opponent</th>
                        <th>Time</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr v-for="gr in scheduledGames" :key="gr.gameReportId">
                        <td v-if="refTeamId !== null && refTeamId > 0" v-once>{{ formatGameDate(gr.gameDate) }}</td>
                        <td v-if="selectedSportId === null || selectedSportId === 0">{{ gr.sport.name }}</td>
                        <td v-once>{{ formatOpponent(gr) }}</td>
                        <td v-once>{{ formatGameTime(gr.gameDate) }}</td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Pre-Game</a></td>
                    </tr>
                </tbody>
            </table>

            <!-- No Ref Display -->
            <table class="table table-striped table-borderless" v-if="!refDisplay && (finalGames !== null && finalGames.length > 0 || scheduleGames !== null && scheduledGames.length > 0)">
                <thead v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr>
                        <th v-if="selectedSportId === null || selectedSportId === 0">Sport</th>
                        <th colspan="2">Matchup</th>
                        <th>Time</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr v-for="gr in scheduledGames" :key="gr.gameReportId">
                        <td v-once v-if="selectedSportId === null || selectedSportId === 0">{{ gr.sport.name }}</td>
                        <td v-once>{{ getAwayTeam(gr) }}</td>
                        <td v-once>{{ getHomeTeam(gr) }}</td>
                        <td v-once>{{ formatGameTime(gr.gameDate) }}</td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Pre-Game</a></td>
                    </tr>
                </tbody>
                <thead v-if="finalGames !== null && finalGames.length > 0">
                    <tr>
                        <th v-if="selectedSportId === null || selectedSportId === 0">Sport</th>
                        <th>Winner</th>
                        <th>Opponent</th>
                        <th>Result</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="finalGames !== null && finalGames.length > 0">
                    <tr v-for="gr in finalGames" :key="gr.gameReportId">
                        <td v-once v-if="selectedSportId === null || selectedSportId === 0">{{ gr.sport.name }}</td>
                        <td v-once>{{ getWinningTeam(gr) }}</td>
                        <td v-once>{{ getLosingTeam(gr) }}</td>
                        <td v-once v-html="formatResult(gr)"></td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Game Details</a></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="card-body" v-else>
            <p>There are no games on {{ formatDisplayDate(activeDate) }}.</p>
        </div>
    </div>
</template>

<script>
    import VCalendar from "v-calendar";

    export default {
        name: "schedule",
        components: {
            VCalendar,
        },
        props: {
            sportId: {
                type: Number,
                default: null,
            },
            schoolId: {
                type: Number,
                default: null
            },
            scheduleYearId: {
                type: Number,
                default: null
            },
            refTeamId: {
                type: Number,
                default: null
            },
            defaultDate: {
                type: String,
                default: null
            },
            lowScoreWins: {
                type: Boolean,
                default: false
            },
            historyBasePath: {
                type: String,
                required: "",
            }
        },
        data() {
            return {
                selectedSportId: this.sportId,
                selectedSchoolId: this.schoolId,
                selectedScheduleYearId: this.scheduleYearId,
                viewDate: this.defaultDate !== null && this.defaultDate !== "" ? moment(this.defaultDate) : null,
                activeDate: this.defaultDate !== null && this.defaultDate !== "" ? moment(this.defaultDate) : null,
                scheduleData: null,
                loading: false,
            }
        },
        computed: {
            refDisplay() {
                return (this.selectedSchoolId !== null && this.selectedSchoolId > 0 || (this.selectedSportId !== null && this.selectedSportId > 0 &&
                    this.selectedSchoolId !== null && this.selectedSchoolId > 0 &&
                    this.selectedScheduleYearId !== null && this.selectedScheduleYearId > 0));
            },

            refTeam() {
                return ((this.selectedSportId !== null && this.selectedSportId > 0 &&
                    this.selectedSchoolId !== null && this.selectedSchoolId > 0 &&
                    this.selectedScheduleYearId !== null && this.selectedScheduleYearId > 0));
            },

            finalGames() {
                let dt = moment();
                return this.scheduleData.games.filter(gr => gr.hasFinalScore);
            },

            scheduledGames() {
                let dt = moment();
                return this.scheduleData.games.filter(gr => !gr.hasFinalScore && moment(gr.gameDate).isAfter(dt));
            }
        },

        mounted() {
            console.log("default date as moment:", moment(this.defaultDate));
            this.loadSchedule();
        },

        methods: {
            loadSchedule() {
                var vm = this;
                vm.loading = true;
                // initialize our data using the params that were passed in
                $.get("/siteapi/games/schedule",
                    {
                        viewStart: this.viewDate !== null ? this.viewDate.format("M/D/YYYY") : null,
                        sportId: this.selectedSportId,
                        schoolId: this.selectedSchoolId,
                        scheduleYearId: this.selectedScheduleYearId
                    },
                    (data) => { // success
                        if (data !== undefined && data !== null && data.viewDate) {
                            vm.activeDate = moment(data.viewDate);
                        }
                        vm.scheduleData = data;

                        vm.loading = false;
                    }, "json");
            },

            getDayName(dateVal) {
                var dt = moment(dateVal);
                return dt.format("ddd");
            },

            formatDisplayDate(dateVal) {
                var dt = moment(dateVal);
                return dt.format("M/D");
            },

            formatGameDate(dateVal) {
                var dt = moment(dateVal);
                return dt.format("M/D");
            },

            formatGameTime(dateVal) {
                let ret = "TBA";
                var dt = moment(dateVal);
                if (dt.hours() !== 0) {
                    ret = dt.format("h:mm a");
                }
                return ret;
            },

            formatOpponent(game) {
                let ret = "";

                if (this.refTeamId !== null && this.refTeamId > 0) {
                    let refTeam = game.teams.find(t => t.teamId === this.refTeamId);
                    let oppTeam = game.teams.find(t => t.teamId !== this.refTeamId);
                    let oppTeamName = oppTeam !== undefined && oppTeam !== null ? oppTeam.name : "TBA";
                    if (refTeam.homeTeam) {
                        ret = "vs. " + oppTeamName;
                    } else {
                        ret = "@ " + oppTeamName;
                    }
                } else if (this.selectedSchoolId !== null && this.selectedSchoolId > 0) {
                    var refTeam = game.teams.find(t => t.schoolId === this.selectedSchoolId);
                    var oppTeam = game.teams.find(t => t.schoolId !== this.selectedSchoolId);
                    let oppTeamName = oppTeam !== undefined && oppTeam !== null ? oppTeam.name : "TBA";
                    if (refTeam.homeTeam) {
                        ret = "vs. " + oppTeamName;
                    } else {
                        ret = "@ " + oppTeamName;
                    }
                }
                return ret;
            },

            getWinningTeam(game) {
                let ret = "";

                let lowScoreWins = game.sport.enableLowScoreWins;
                let winningTeam = null;
                if (lowScoreWins) {
                    winningTeam = game.teams.reduce(function (prev, curr) {
                        return prev.finalScore < curr.finalScore ? prev : curr;
                    });
                } else {
                    winningTeam = game.teams.reduce(function (prev, curr) {
                        return prev.finalScore > curr.finalScore ? prev : curr;
                    });
                }

                if (winningTeam !== undefined && winningTeam !== null) {
                    ret = winningTeam.name;
                }

                return ret;
            },

            getLosingTeam(game) {
                let ret = "";

                let lowScoreWins = game.sport.enableLowScoreWins;
                let losingTeam = null;
                if (lowScoreWins) {
                    losingTeam = game.teams.reduce(function (prev, curr) {
                        return prev.finalScore > curr.finalScore ? prev : curr;
                    });
                } else {
                    losingTeam = game.teams.reduce(function (prev, curr) {
                        return prev.finalScore < curr.finalScore ? prev : curr;
                    });
                }

                if (losingTeam !== undefined && losingTeam !== null) {
                    let homeTeam = game.teams.find(t => t.homeTeam);
                    let locPrefix = "@ ";
                    if (homeTeam.teamId === losingTeam.teamId) {
                        locPrefix = "vs. ";
                    }
                    ret = locPrefix + losingTeam.name;
                }

                return ret;
            },

            getAwayTeam(game) {
                let ret = "";

                let awayTeam = game.teams.find(t => !t.homeTeam);
                if (awayTeam !== undefined && awayTeam !== null) {
                    ret = awayTeam.name;
                    if (game.hasFinalScore) {
                        ret += " (" + awayTeam.finalScore + ")";
                    }
                }

                return ret;
            },

            getHomeTeam(game) {
                let ret = "";

                let homeTeam = game.teams.find(t => t.homeTeam);
                if (homeTeam !== undefined && homeTeam !== null) {
                    ret = "@ " + homeTeam.name;
                    if (game.hasFinalScore) {
                        ret += " (" + homeTeam.finalScore + ")";
                    }
                }

                return ret;
            },

            formatResult(game) {
                let ret = "";

                if (this.refTeamId !== null && this.refTeamId > 0) {
                    let refTeam = game.teams.find(t => t.teamId === this.refTeamId);
                    let oppTeam = game.teams.find(t => t.teamId !== this.refTeamId);
                    let refScore = refTeam.finalScore;
                    let oppScore = oppTeam !== undefined && oppTeam !== null ? oppTeam.finalScore : 0;

                    if (this.lowScoreWins) {
                        if (refScore < oppScore) {
                            ret = '<span class="win">W</span> ' + refScore + "-" + oppScore;
                        } else {
                            ret = '<span class="loss">L</span> ' + oppScore + "-" + refScore;
                        }
                    } else {
                        if (refScore > oppScore) {
                            ret = '<span class="win">W</span> ' + refScore + "-" + oppScore;
                        } else {
                            ret = '<span class="loss">L</span> ' + oppScore + "-" + refScore;
                        }
                    }
                } else if (this.selectedSchoolId !== null && this.selectedSchoolId > 0) {
                    let refTeam = game.teams.find(t => t.schoolId === this.selectedSchoolId);
                    let oppTeam = game.teams.find(t => t.schoolId !== this.selectedSchoolId);
                    let refScore = refTeam.finalScore;
                    let oppScore = oppTeam !== undefined && oppTeam !== null ? oppTeam.finalScore : 0;

                    if (this.lowScoreWins) {
                        if (refScore < oppScore) {
                            ret = '<span class="win">W</span> ' + refScore + "-" + oppScore;
                        } else {
                            ret = '<span class="loss">L</span> ' + oppScore + "-" + refScore;
                        }
                    } else {
                        if (refScore > oppScore) {
                            ret = '<span class="win">W</span> ' + refScore + "-" + oppScore;
                        } else {
                            ret = '<span class="loss">L</span> ' + oppScore + "-" + refScore;
                        }
                    }
                } else {
                    let homeTeam = game.teams.find(t => t.homeTeam);
                    let awayTeam = game.teams.find(t => !t.homeTeam);
                    let homeScore = homeTeam !== undefined && homeTeam !== null ? homeTeam.finalScore : 0;
                    let awayScore = awayTeam !== undefined && awayTeam !== null ? awayTeam.finalScore : 0;
                    if (this.lowScoreWins) {
                        if (homeScore < awayScore) {
                            ret = homeScore + "-" + awayScore;
                        } else {
                            ret = awayScore + "-" + homeScore;
                        }
                    } else {
                        if (homeScore > awayScore) {
                            ret = homeScore + "-" + awayScore;
                        } else {
                            ret = awayScore + "-" + homeScore;
                        }
                    }
                }

                return ret;
            },

            setViewDate(ev) {
                let dt = moment(ev);
                this.viewDate = dt;

                //TODO: Set the schedule date pref load in the URL
                if (this.historyBasePath !== null && this.historyBasePath !== "") {
                    history.pushState({
                        id: "scheduleView" + dt.format("YYYYMMDD")
                    }, document.title, this.historyBasePath + "/" + dt.format("YYYY") + "/" + dt.format("MM") + "/" + dt.format("DD"));
                }


                this.loadSchedule();
            }
        }
    };
</script>