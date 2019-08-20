<template>
    <div class="schedule card" v-if="scheduleData !== undefined && scheduleData !== null">
        <div class="card-body">
            <h1><strong>Schedule</strong> / Scoreboard</h1>
        </div>
        <div class="card-body border-top-0" v-if="!teamDisplay">
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
            <table class="table table-striped table-borderless" v-if="refDisplay && (finalGames !== null && finalGames.length > 0 || scheduleGames !== null && scheduledGames.length > 0)">
                <thead v-if="finalGames !== null && finalGames.length > 0">
                    <tr>
                        <th v-if="refTeamId !== null && refTeamId > 0">Date</th>
                        <th v-if="selectedSchoolId !== null && selectedSchoolId > 0">Sport</th>
                        <th>Opponent</th>
                        <th>Result</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="finalGames !== null && finalGames.length > 0">
                    <tr v-for="gr in finalGames" :key="gr.gameReportId">
                        <td v-if="refTeamId !== null && refTeamId > 0" v-once>{{ formatGameDate(gr.gameDate) }}</td>
                        <td v-if="selectedSchoolId !== null && selectedSchoolId > 0">{{ gr.sport.name }}</td>
                        <td v-once>{{ formatOpponent(gr) }}</td>
                        <td v-once v-html="formatResult(gr)"></td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Game Details</a></td>
                    </tr>
                </tbody>
                <thead v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr>
                        <th v-if="refTeamId !== null && refTeamId > 0">Date</th>
                        <th v-if="selectedSchoolId !== null && selectedSchoolId > 0">Sport</th>
                        <th>Opponent</th>
                        <th>Time</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr v-for="gr in scheduledGames" :key="gr.gameReportId">
                        <td v-if="refTeamId !== null && refTeamId > 0" v-once>{{ formatGameDate(gr.gameDate) }}</td>
                        <td v-if="selectedSchoolId !== null && selectedSchoolId > 0">{{ gr.sport.name }}</td>
                        <td v-once>{{ formatOpponent(gr) }}</td>
                        <td v-once>{{ formatGameTime(gr.gameDate) }}</td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Pre-Game</a></td>
                    </tr>
                </tbody>
            </table>

            <table class="table table-striped table-borderless" v-if="!refDisplay && (finalGames !== null && finalGames.length > 0 || scheduleGames !== null && scheduledGames.length > 0)">
                <thead v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr>
                        <th colspan="2">Matchup</th>
                        <th>Time</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr v-for="gr in scheduledGames" :key="gr.gameReportId">
                        <td v-once>{{ getAwayTeam(gr) }}</td>
                        <td v-once>{{ getHomeTeam(gr) }}</td>
                        <td v-once>{{ formatGameTime(gr.gameDate) }}</td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Pre-Game</a></td>
                    </tr>
                </tbody>
                <thead v-if="finalGames !== null && finalGames.length > 0">
                    <tr>
                        <th colspan="2">Matchup</th>
                        <th>Result</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="finalGames !== null && finalGames.length > 0">
                    <tr v-for="gr in finalGames" :key="gr.gameReportId">
                        <td v-once>{{ getAwayTeam(gr) }}</td>
                        <td v-once>{{ getHomeTeam(gr) }}</td>
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
                type: Date,
                default: null
            },
            lowScoreWins: {
                type: Boolean,
                default: false
            }
        },
        data() {
            return {
                selectedSportId: this.sportId,
                selectedSchoolId: this.schoolId,
                selectedScheduleYearId: this.scheduleYearId,
                viewDate: this.defaultDate,
                activeDate: this.defaultDate !== null ? moment(this.defaultDate) : null,
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

            finalGames() {
                let dt = moment();
                return this.scheduleData.games.filter(gr => gr.hasFinalScore);
            },

            scheduledGames() {
                let dt = moment();
                return this.scheduleData.games.filter(gr => !gr.hasFinalScore && moment(gr.gameDate).isAfter(dt));
            }
        },

        watch: {
            sportId(newVal) {
                this.selectedSportId = newVal;
            },
            schoolId(newVal) {
                this.selectedSchoolId = newVal;
            },
            scheduleYearId(newVal) {
                this.selectedScheduleYearId = newVal;
            },
        },

        mounted() {
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
                return dt.format("M/D/YYYY");
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
                    var refTeam = game.teams.find(t => t.teamId === this.refTeamId);
                    var oppTeam = game.teams.find(t => t.teamId !== this.refTeamId);
                    if (refTeam.homeTeam) {
                        ret = "vs. " + oppTeam.name;
                    } else {
                        ret = "@ " + oppTeam.name;
                    }
                } else if (this.selectedSchoolId !== null && this.selectedSchoolId > 0) {
                    var refTeam = game.teams.find(t => t.schoolId === this.selectedSchoolId);
                    var oppTeam = game.teams.find(t => t.schoolId !== this.selectedSchoolId);
                    if (refTeam.homeTeam) {
                        ret = "vs. " + oppTeam.name;
                    } else {
                        ret = "@ " + oppTeam.name;
                    }
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
                    let oppScore = oppTeam.finalScore;

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
                    let oppScore = oppTeam.finalScore;

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
                    let homeScore = homeTeam.finalScore;
                    let awayScore = awayTeam.finalScore;
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
                //history.pushState({
                //    id: "scheduleView" + dt.format("YYYYMMDD")
                //}, document.title, 'http://my-app-url.com/?p=homepage');


                this.loadSchedule();
            }
        }
    };
</script>