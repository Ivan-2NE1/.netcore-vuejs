<template>
    <div class="schedule card" v-if="scheduleData !== undefined && scheduleData !== null">
        <div class="card-body">
            <h1><strong>Schedule</strong> / Scoreboard</h1>
        </div>
        
        <div class="card-body p-0" v-if="scheduleData.games !== null && scheduleData.games.length > 0">
            <table class="table table-striped table-borderless" v-if="finalGames !== null && finalGames.length > 0 || scheduleGames !== null && scheduledGames.length > 0">
                <thead v-if="finalGames !== null && finalGames.length > 0">
                    <tr>
                        <th>Date</th>
                        <th>Sport</th>
                        <th>Opponent</th>
                        <th>Result</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="finalGames !== null && finalGames.length > 0">
                    <tr v-for="gr in finalGames" :key="gr.gameReportId">
                        <td v-once>{{ formatGameDate(gr.gameDate) }}</td>
                        <td v-once>{{ gr.sport.name }}</td>
                        <td v-once>{{ formatOpponent(gr) }}</td>
                        <td v-once v-html="formatResult(gr)"></td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Game Details</a></td>
                    </tr>
                </tbody>
                <thead v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr>
                        <th>Date</th>
                        <th>Sport</th>
                        <th>Opponent</th>
                        <th>Time</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody v-if="scheduledGames !== null && scheduledGames.length > 0">
                    <tr v-for="gr in scheduledGames" :key="gr.gameReportId">
                        <td v-once>{{ formatGameDate(gr.gameDate) }}</td>
                        <td v-once>{{ gr.sport.name }}</td>
                        <td v-once>{{ formatOpponent(gr) }}</td>
                        <td v-once>{{ formatGameTime(gr.gameDate) }}</td>
                        <td v-once><a :href="'/game/' + gr.gameReportId">Pre-Game</a></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="card-body" v-else>
            <p>There are no games found.</p>
        </div>
    </div>
</template>

<script>
    import VCalendar from "v-calendar";

    export default {
        name: "team-schedule",
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
                scheduleData: null,
                loading: false,
            }
        },
        computed: {
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
            this.loadSchedule();
        },

        methods: {
            loadSchedule() {
                var vm = this;
                vm.loading = true;
                // initialize our data using the params that were passed in
                $.get("/siteapi/games/schedule",
                    {
                        viewStart: null,
                        sportId: this.selectedSportId,
                        schoolId: this.selectedSchoolId,
                        scheduleYearId: this.selectedScheduleYearId
                    },
                    (data) => { // success
                        vm.scheduleData = data;
                        vm.loading = false;
                    }, "json");
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
        }
    };
</script>