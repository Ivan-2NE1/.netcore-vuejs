<template>
    <div class="card event-scoring">
        <div class="card-body">
            <div class="row">
                <div class="col-md-4">
                    <div class="custom-control custom-radio mb-1">
                        <input type="radio" class="custom-control-input"
                               v-bind:id="'MatchWinner0-' + eventResult.eventResultId"
                               v-bind:key="'MatchWinner0-' + eventResult.eventResultId"
                               v-bind:name="'MatchWinner-' + eventResult.eventResultId"
                               v-bind:value="0"
                               v-bind:checked="getMatchWinner(0) === true"
                               v-on:change="setMatchWinner(0)" />
                        <label class="custom-control-label" v-bind:for="'MatchWinner0-' + eventResult.eventResultId">Match Winner</label>
                    </div>

                    <autocomplete v-bind:source="roster"
                                  results-display="display"
                                  v-bind:ref="'autocomplete-p00-' + eventResult.eventResultId"
                                  v-bind:initial-value="eventResult.groups[0].players[0].playerId"
                                  v-bind:initial-display="getPlayerDisplay(eventResult.groups[0].players[0].playerId)"
                                  results-value="playerId"
                                  input-class="form-control"
                                  placeholder="Search for Player"
                                  clear-button-icon="fas fa-times"
                                  v-on:selected="setSelectedPlayer(0, $event.selectedObject)">
                        <template slot="noResults" v-on:click.prevent="showAddPlayer = true">
                            <i class="far fa-plus-circle"></i> Create a new player
                        </template>
                    </autocomplete>
                    <autocomplete v-bind:source="roster"
                                  results-display="display"
                                  v-bind:ref="'autocomplete-p01-' + eventResult.eventResultId"
                                  v-bind:initial-value="eventResult.groups[0].players[1].playerId"
                                  v-bind:initial-display="getPlayerDisplay(eventResult.groups[0].players[1].playerId)"
                                  results-value="playerId"
                                  input-class="form-control"
                                  placeholder="Search for Player"
                                  clear-button-icon="fas fa-times"
                                  v-on:selected="setSelectedPlayer(0, $event.selectedObject)">
                        <template slot="noResults" v-on:click.prevent="showAddPlayer = true">
                            <i class="far fa-plus-circle"></i> Create a new player
                        </template>
                    </autocomplete>
                </div>
                <div class="col-md-4">
                    <input-select choose-prompt="Result Type" label="Choose Result Type"
                                  v-bind:default-value="eventResult.resultType"
                                  v-bind:required="true"
                                  v-bind:options="resultTypeOptions"
                                  v-on:change="onEventResultTypeChanged"></input-select>
                </div>
                <div class="col-md-4">
                    <div class="custom-control custom-radio mb-1">
                        <input type="radio" class="custom-control-input"
                               v-bind:id="'MatchWinner1-' + eventResult.eventResultId"
                               v-bind:key="'MatchWinner1-' + eventResult.eventResultId"
                               v-bind:name="'MatchWinner-' + eventResult.eventResultId"
                               v-bind:value="1"
                               v-bind:checked="getMatchWinner(1) === true"
                               v-on:change="setMatchWinner(1)" />
                        <label class="custom-control-label" v-bind:for="'MatchWinner1-' + eventResult.eventResultId">Match Winner</label>
                    </div>
                    <autocomplete v-bind:source="roster"
                                  results-display="display"
                                  v-bind:ref="'autocomplete-p10-' + eventResult.eventResultId"
                                  v-bind:initial-value="eventResult.groups[1].players[0].playerId"
                                  v-bind:initial-display="getPlayerDisplay(eventResult.groups[1].players[0].playerId)"
                                  results-value="playerId"
                                  input-class="form-control"
                                  placeholder="Search for Player"
                                  clear-button-icon="fas fa-times"
                                  v-on:selected="setSelectedPlayer(1, $event.selectedObject)">
                        <template slot="noResults" v-on:click.prevent="showAddPlayer = true">
                            <i class="far fa-plus-circle"></i> Create a new player
                        </template>
                    </autocomplete>
                    <autocomplete v-bind:source="roster"
                                  results-display="display"
                                  v-bind:ref="'autocomplete-p11-' + eventResult.eventResultId"
                                  v-bind:initial-value="eventResult.groups[1].players[1].playerId"
                                  v-bind:initial-display="getPlayerDisplay(eventResult.groups[1].players[1].playerId)"
                                  results-value="playerId"
                                  input-class="form-control"
                                  placeholder="Search for Player"
                                  clear-button-icon="fas fa-times"
                                  v-on:selected="setSelectedPlayer(1, $event.selectedObject)">
                        <template slot="noResults" v-on:click.prevent="showAddPlayer = true">
                            <i class="far fa-plus-circle"></i> Create a new player
                        </template>
                    </autocomplete>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" v-for="idx in 3">
                    <table class="table table-sm">
                        <caption style="caption-side:top">
                            <strong>Set {{ idx }}</strong> <label class="checkbox inline float-right">
                                <input-checkbox v-bind:default-value="isTieBreak(idx)"
                                                v-bind:label="'Set ' + idx + ' Tie-Break'"
                                                v-on:input="setEventSetTiebreak(idx, $event)"></input-checkbox>
                            </label>
                        </caption>
                        <thead>
                            <tr>
                                <th width="50%"><span class="d-block text-truncate">{{ firstTeamName }}</span></th>
                                <th width="50%"><span class="d-block text-truncate">{{ secondTeamName }}</span></th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Match Score -->
                            <tr>
                                <td>
                                    <div class="form-inline input-number">
                                        <input-field v-bind:input-id="'eventresultstat-' + eventResult.eventResultId + '-0'"
                                                     input-type="number"
                                                     v-bind:default-value="getEventGroupStatValue(0, idx, false)"
                                                     v-on:change="setEventGroupStatValue(0, idx, false, $event)">
                                        </input-field>
                                    </div>
                                </td>
                                <td>
                                    <div class="form-inline input-number">
                                        <input-field v-bind:input-id="'eventresultstat-' + eventResult.eventResultId + '-1'"
                                                     input-type="number"
                                                     v-bind:default-value="getEventGroupStatValue(1, idx, false)"
                                                     v-on:change="setEventGroupStatValue(1, idx, false, $event)">
                                        </input-field>
                                    </div>
                                </td>
                            </tr>
                            <!-- Tie Break -->
                            <tr v-if="isTieBreak(idx)">
                                <td>
                                    <div class="form-inline input-number">
                                        <input-field label="TB" v-bind:input-id="'eventresultstattb-' + eventResult.eventResultId + '-0'"
                                                     input-type="number"
                                                     v-bind:default-value="getEventGroupStatValue(0, idx, true)"
                                                     v-on:change="setEventGroupStatValue(0, idx, true, $event)">
                                        </input-field>
                                    </div>
                                </td>
                                <td>
                                    <div class="form-inline input-number">
                                        <input-field label="TB" v-bind:input-id="'eventresultstattb-' + eventResult.eventResultId + '-1'"
                                                     input-type="number"
                                                     v-bind:default-value="getEventGroupStatValue(1, idx, true)"
                                                     v-on:change="setEventGroupStatValue(1, idx, true, $event)">
                                        </input-field>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="card-body">
            <button class="btn btn-primary" v-bind:disabled="matchInvalid">Save Match</button>
        </div>
    </div>
</template>

<script>
    import Autocomplete from "../base/autocomplete/autocomplete.vue";
    import InputField from "../base/input-field/input-field.vue";
    import InputCheckbox from "../base/input-checkbox/input-checkbox.vue";
    import InputSelect from "../base/input-select/input-select.vue";

    export default {
        name: "event-result-tennis-doubles",
        components: {
            "autocomplete": Autocomplete,
            "input-field": InputField,
            "input-checkbox": InputCheckbox,
            "input-select": InputSelect,
        },

        props: {
            gameEventResult: {
                type: Object,
                required: true,
            },
            rosterEntries: {
                type: Array,
                required: true,
            },
            resultTypes: {
                type: Array,
                required: true,
            },
            sportEventStats: {
                type: Array,
                required: true,
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                eventResult: this.gameEventResult,
                roster: this.rosterEntries,
                showAddPlayer: false,
                set1tb: false,
                set2tb: false,
                set3tb: false,
            }
        },

        computed: {
            resultTypeOptions() {
                let rt = [];

                rt = this.resultTypes.map(r => {
                    return r.name;
                });

                return rt;
            },

            firstTeamName() {
                let wgroup = this.eventResult.groups[0];
                if (wgroup !== undefined && wgroup !== null) {
                    let wgroupplayer = wgroup.players.find(p => p.playerId !== null && p.playerId > 0);
                    if (wgroupplayer !== undefined && wgroupplayer !== null) {
                        let player = this.roster.find(r => r.playerId === wgroupplayer.playerId);
                        if (player !== undefined && player !== null) {
                            return player.teamName;
                        }
                    }
                }
                return "";
            },

            secondTeamName() {
                let lgroup = this.eventResult.groups[1];
                if (lgroup !== undefined && lgroup !== null) {
                    let lgroupplayer = lgroup.players.find(p => p.playerId !== null && p.playerId > 0);
                    if (lgroupplayer !== undefined && lgroupplayer !== null) {
                        let player = this.roster.find(r => r.playerId === lgroupplayer.playerId);
                        if (player !== undefined && player !== null) {
                            return player.teamName;
                        }
                    }
                }
                return "";
            },

            matchInvalid() {
                let ret = false;

                if (this.eventResult.groups[0].gameReportTeamId === this.eventResult.groups[1].gameReportTeamId) {
                    ret = true;
                }

                return ret;
            }

        },

        created() {
            var vm = this;

            this.set1tb = this.getEventSetTiebreak(1);
            this.set2tb = this.getEventSetTiebreak(2);
            this.set3tb = this.getEventSetTiebreak(3);
        },

        watch: {
            gameEventResult(newval) {
                this.eventResult = newval;
            },
            rosterEntries(newval) {
                this.roster = newval;
            },
        },

        methods: {
            getPlayerDisplay(playerId) {
                let display = null;
                if (playerId !== undefined && playerId !== null && playerId > 0) {
                    let player = this.rosterEntries.find(re => re.playerId === playerId);
                    if (player !== undefined && player !== null) {
                        display = player.display;
                    }
                }
                return display;
            },

            setSelectedPlayer(idx, selected) {
                let player = this.eventResult.players[idx];
                if (player !== undefined && player !== null) {
                    player.playerId = selected.playerId;
                }
            },

            getMatchWinner(idx) {
                let result = false;
                let group = this.eventResult.groups[idx];
                if (group !== undefined && group !== null) {
                    result = group.winner;
                }
                return result;
            },

            setMatchWinner(idx) {
                for (const [i, g] of this.eventResult.groups.entries()) {
                    g.winner = false;
                    if (i === idx) {
                        g.winner = true;
                    }
                }
            },

            onEventResultTypeChanged(e) {
                this.eventResult.resultType = e.value;
            },

            isTieBreak(setNumber) {
                return this["set" + setNumber + "tb"];
            },

            getEventSetTiebreak(setNumber) {
                var tb = false;

                let tbSet = this.sportEventStats.find(ses => ses.abbreviation == setNumber + "TB");
                if (tbSet !== undefined && tbSet !== null) {
                    let seStatId = tbSet.sportEventStatId;

                    for (let i = 0; i < this.eventResult.groups.length; i++) {
                        let tbStats = this.eventResult.groups[i].stats.find(s => s.statId === seStatId && s.statValue > 0);
                        if (tbStats !== undefined && tbStats !== null) {
                            tb = true;
                            break;
                        }
                    }
                }
                return tb;
            },

            setEventSetTiebreak(setNumber, value) {
                this["set" + setNumber + "tb"] = value;
            },

            getEventGroupStatValue(groupIdx, setNumber, isTiebreak) {
                var val = 0;

                let set = this.sportEventStats.find(ses => ses.abbreviation == setNumber + (isTiebreak ? "TB" : ""));
                if (set !== undefined && set !== null) {
                    let seStatId = set.sportEventStatId;
                    let group = this.gameEventResult.groups[groupIdx];
                    let setStat = group.stats.find(s => s.statId === seStatId);
                    if (setStat !== undefined && setStat !== null) {
                        val = setStat.statValue;
                    }
                }
                return val;
            },

            setEventGroupStatValue(groupIdx, setNumber, isTiebreak, value) {
                let set = this.sportEventStats.find(ses => ses.abbreviation == setNumber + (isTiebreak ? "TB" : ""));
                if (set !== undefined && set !== null) {
                    let seStatId = set.sportEventStatId;
                    let group = this.gameEventResult.groups[groupIdx];
                    let setStat = group.stats.find(s => s.statId === seStatId);
                    if (setStat !== undefined && setStat !== null) {
                        setStat.statValue = Number(value);
                    }
                }
            }
        }
    };
</script>