<template>
    <widget-wrapper icon="tally" title="Game Scoring"
                    v-bind:padding="false">
        <template v-slot:toolbar>
            <button class="btn btn-default btn-sm rounded-pill mr-1 shadow-0 waves-effect waves-themed d-none d-md-inline"
                    v-if="game.sport.allowOvertime"
                    v-on:click.prevent="addOvertime">
                <i class="fal fa-plus"></i> Add {{ game.sport.overtimeName }}
            </button>

            <button class="btn btn-default btn-sm rounded-pill mr-1 shadow-0 waves-effect waves-themed d-none d-md-inline"
                    v-if="game.sport.enableShootout"
                    v-on:click.prevent="addOvertime">
                <i class="fal fa-plus"></i> Add Shootout
            </button>
        </template>

        <template v-slot:toolbarmasterbuttons>
            <game-nav v-bind:game-report-id="game.gameReportId"></game-nav>
        </template>

        <template v-slot:bodymessage>
            <div class="alert alert-info" v-if="isBaseball">
                Enter an "X" in un-played innings.
            </div>
        </template>

        <div class="d-md-none text-center pt-2 mb-2">
            <button class="btn btn-default btn-sm rounded-pill mr-1 shadow-0 waves-effect waves-themed"
                    v-if="game.sport.allowOvertime"
                    v-on:click.prevent="addOvertime">
                <i class="fal fa-plus"></i> Add {{ game.sport.overtimeName }}
            </button>

            <button class="btn btn-default btn-sm rounded-pill mr-1 shadow-0 waves-effect waves-themed"
                    v-if="game.sport.enableShootout"
                    v-on:click.prevent="addOvertime">
                <i class="fal fa-plus"></i> Add Shootout
            </button>
        </div>

        <table class="table table-bordered table-striped mb-0 responsive">
            <thead>
                <tr>
                    <th>&nbsp;</th>
                    <th v-for="periodNumber in numberOfPeriods">
                        {{ periodNumber | ordinal }}<span v-if="periodIsSo(periodNumber)"> (SO)</span><span v-else-if="periodIsOt(periodNumber)"> (OT)</span>
                    </th>
                    <th class="table-success">Final</th>
                    <th v-if="isBaseball">Hits</th>
                    <th v-if="isBaseball">Errors</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="team in sortedTeams">
                    <td data-title="Team">{{ team.name }}</td>
                    <td v-for="periodNumber in numberOfPeriods"
                        v-bind:data-title="$options.filters.ordinal(periodNumber) + ' ' + game.sport.periodName + (periodIsSo(periodNumber) ? ' (SO)' : periodIsOt(periodNumber) ? ' (OT)' : '')">
                        <div class="form-inline input-number">
                            <input-field v-bind:input-id="'periodscore' + periodNumber + '-' + team.gameReportTeamId"
                                         v-bind:default-value="getTeamPeriodScore(team.gameReportTeamId, periodNumber)"
                                         v-on:input="setTeamPeriodScore(team.gameReportTeamId, periodNumber, $event)"></input-field>
                        </div>
                    </td>
                    <td data-title="Final" class="table-success">
                        <div class="form-inline input-number">
                            <input-field v-bind:input-id="'FinalScore' + team.gameReportTeamId"
                                         v-bind:default-value="team.finalScore"
                                         v-on:input="setTeamScore(team.gameReportTeamId, $event)"></input-field>
                        </div>
                    </td>
                    <td v-if="isBaseball" data-title="Hits">
                        <div class="form-inline input-number">
                            <input-field v-bind:input-id="'Hits' + team.gameReportTeamId"
                                         v-bind:default-value="0"
                                         v-on:input="setTeamHits(team.gameReportTeamId, $event)"></input-field>
                        </div>
                    </td>
                    <td v-if="isBaseball" data-title="Errors">
                        <div class="form-inline input-number">
                            <input-field v-bind:input-id="'Errors' + team.gameReportTeamId"
                                         v-bind:default-value="0"
                                         v-on:input="setTeamErrors(team.gameReportTeamId, $event)"></input-field>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </widget-wrapper>
</template>

<script>
    import WidgetWrapper from "../base/widget-wrapper/widget-wrapper.vue";
    import InputField from "../base/input-field/input-field.vue";
    import GameNav from "./nav.vue";

    export default {
        name: "scoring",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },

        components: {
            "widget-wrapper": WidgetWrapper,
            "input-field": InputField,
            "game-nav": GameNav,
        },

        props: {
            gameReport: {
                type: Object,
                required: true,
            },
        },

        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                game: this.gameReport,
            }
        },

        computed: {
            // Computed properties
            numberOfPeriods() {
                return this.game.teams.reduce((prev, curr) => {
                    let maxTeamPs = curr.periodScores.reduce((max, ps) => ps.periodNumber > max ? ps.periodNumber : max, 0);
                    return (maxTeamPs > prev) ? maxTeamPs : prev;
                }, 0);
            },

            sortedTeams() {
                const compare = (a, b) => {
                    let comparison = 0;
                    if (a.homeTeam) {
                        comparison - 1;
                    } else {
                        // name comparison
                        let aName = a.name.toUpperCase();
                        let bName = b.name.toUpperCase();
                        if (aName > bName) {
                            comparison = 1;
                        } else if (aName < bName) {
                            comparison = -1;
                        }
                    }

                    return comparison;
                }
                let lt = JSON.parse(JSON.stringify(this.game.teams));
                lt.sort(compare);
                return lt;
            },

            isBaseball() {
                let name = this.game.sport.name.toLowerCase();
                return name.indexOf("baseball") >= 0 || name.indexOf("softball") >= 0;
            },
        },

        created() {
            // Load whatever we need to get via ajax (not included in the Model)
            // Setup any non-reactive properties here
            // load the sport + game meta configuration
            var vm = this;

        },

        watch: {
            gameReport(newVal, oldVal) {
                this.game = newVal;
            },
        },

        methods: {
            // Do stuff!
            periodIsOt(periodNumber) {
                let ret = false;
                let team = this.game.teams[0];
                if (team !== undefined && team !== null) {
                    let period = team.periodScores.find(ps => ps.periodNumber === periodNumber);
                    if (period !== undefined && period !== null) {
                        ret = period.isOvertimePeriod;
                    }
                }
                return ret;
            },

            periodIsSo(periodNumber) {
                let ret = false;
                let team = this.game.teams[0];
                let period = team.periodScores.find(ps => ps.periodNumber === periodNumber);
                if (period !== undefined && period !== null) {
                    ret = period.isShootOutPeriod;
                }
                return ret;
            },

            getTeamPeriodScore(gameReportTeamId, periodNumber) {
                let periodScore = 0;
                let team = this.game.teams.find(t => t.gameReportTeamId === gameReportTeamId);
                if (team !== undefined && team !== null) {
                    let period = team.periodScores.find(ps => ps.periodNumber === periodNumber);
                    if (period !== undefined && period !== null) {
                        periodScore = period.score;
                        if (period.scoreSpecial !== null && period.scoreSpecial !== "") {
                            periodScore = period.scoreSpecial;
                        }
                    }
                }
                return periodScore;
            },

            setTeamPeriodScore(gameReportTeamId, periodNumber, newValue) {
                let team = this.game.teams.find(t => t.gameReportTeamId === gameReportTeamId);
                if (team !== undefined && team !== null) {
                    let period = team.periodScores.find(ps => ps.periodNumber === periodNumber);
                    if (period !== undefined && period !== null) {
                        if (isNaN(newValue)) {
                            period.score = null;
                            period.scoreSpecial = newValue;
                        } else {
                            period.score = Number(newValue);
                            period.scoreSpecial = "";
                        }
                    }
                }
            },

            setTeamScore(gameReportTeamId, newVal) {
                let team = this.game.teams.find(t => t.gameReportTeamId === gameReportTeamId);
                if (team !== undefined && team !== null) {
                    if (!isNaN(newVal)) {
                        team.finalScore = Number(newVal);
                    } else {
                        console.log("not a number?");
                        // ignore that. invalid
                    }
                }
            },

            addOvertime() {
                let curNumPeriods = this.numberOfPeriods;
                let newNumPeriods = curNumPeriods + 1;


            },

            saveScoring() {

            },
        },
    };
</script>