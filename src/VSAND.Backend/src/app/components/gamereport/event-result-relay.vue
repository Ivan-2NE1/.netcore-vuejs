<template>
    <div class="card event-scoring mb-2">
        <div class="card-body">
            <div class="row mb-2">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label>Name / Team</label>
                        <autocomplete v-bind:source="roster"
                                      results-display="display"
                                      v-bind:ref="'autocomplete-p0-' + eventResult.eventResultId"
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
                                      v-bind:ref="'autocomplete-p0-' + eventResult.eventResultId"
                                      v-bind:initial-value="eventResult.groups[0].players[1].playerId"
                                      v-bind:initial-display="getPlayerDisplay(eventResult.groups[0].players[1].playerId)"
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
                                      v-bind:ref="'autocomplete-p0-' + eventResult.eventResultId"
                                      v-bind:initial-value="eventResult.groups[0].players[2].playerId"
                                      v-bind:initial-display="getPlayerDisplay(eventResult.groups[0].players[2].playerId)"
                                      results-value="playerId"
                                      input-class="form-control"
                                      placeholder="Search for Player"
                                      clear-button-icon="fas fa-times"
                                      v-on:selected="setSelectedPlayer(2, $event.selectedObject)">
                            <template slot="noResults" v-on:click.prevent="showAddPlayer = true">
                                <i class="far fa-plus-circle"></i> Create a new player
                            </template>
                        </autocomplete>
                        <autocomplete v-bind:source="roster"
                                      results-display="display"
                                      v-bind:ref="'autocomplete-p0-' + eventResult.eventResultId"
                                      v-bind:initial-value="eventResult.groups[0].players[3].playerId"
                                      v-bind:initial-display="getPlayerDisplay(eventResult.groups[0].players[3].playerId)"
                                      results-value="playerId"
                                      input-class="form-control"
                                      placeholder="Search for Player"
                                      clear-button-icon="fas fa-times"
                                      v-on:selected="setSelectedPlayer(3, $event.selectedObject)">
                            <template slot="noResults" v-on:click.prevent="showAddPlayer = true">
                                <i class="far fa-plus-circle"></i> Create a new player
                            </template>
                        </autocomplete>
                    </div>
                </div>
                <div class="col-12 col-sm-auto" v-for="stat in sportEventStats">
                    <input-field v-bind:input-id="'eventresultStat-' + stat.sportEventStatId + '-' + eventResult.eventResultId"
                                 v-bind:label="stat.name"
                                 v-bind:input-mask="getStatInputMask(stat)"
                                 v-bind:default-value="getEventPlayerStatValue(stat)"
                                 v-on:change="setEventPlayerStatValue(stat, $event)">
                    </input-field>
                </div>
                <div class="col-sm-4">
                    <div class="input-number">
                        <input-field v-bind:input-id="'eventresultIndplace-' + eventResult.eventResultId"
                                     input-type="number"
                                     label="Place"
                                     v-bind:default-value="eventResult.sortOrder"
                                     v-on:change="setEventPlayerStatPlace($event)">
                        </input-field>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-body">
            Save Button / Delete Button
        </div>
    </div>
</template>

<script>
    import Autocomplete from "../base/autocomplete/autocomplete.vue";
    import InputField from "../base/input-field/input-field.vue";
    import InputCheckbox from "../base/input-checkbox/input-checkbox.vue";
    import InputSelect from "../base/input-select/input-select.vue";
    import VsandInputFormatting from "./VsandInputFormatting";

    export default {
        name: "event-result-relay",
        components: {
            "autocomplete": Autocomplete,
            "input-field": InputField,
            "input-checkbox": InputCheckbox,
            "input-select": InputSelect,
        },
        extends: VsandInputFormatting,

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
            }
        },

        created() {
            var vm = this;

        },

        watch: {
            gameEventResult(newval) {
                this.eventResult = newval;
            }
        },

        methods: {
            getPlayerDisplay(playerId) {
                let display = null;
                if (this.rosterEntries === undefined || this.rosterEntries === null) {
                    return display;
                }

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

            getEventPlayerStatPlace() {

            },

            setEventPlayerStatPlace(newValue) {
                let val = Number(newValue);
                if (!isNaN(val)) {
                    this.eventResult.sortOrder = Number(newValue);
                }
            },

            getStatInputMask(stat) {
                return this.getDataTypeInputMask(stat.dataType);
            },

            getEventPlayerStatValue(refStat) {
                if (this.eventResult.groups === undefined || this.eventResult.groups === null) {
                    return "";
                }

                let statValueRaw = 0;
                let group = this.eventResult.groups[0];
                if (group !== undefined && group !== null) {
                    let stat = group.stats.find(s => s.statId === refStat.sportEventStatId);
                    if (stat !== undefined && stat !== null) {
                        statValueRaw = stat.statValue;
                    }
                }

                let formattedStatValue = this.maskDataTypeValue(statValueRaw, refStat.dataType);
                return formattedStatValue;
            },

            setEventPlayerStatValue(stat) {

            },
        }
    };
</script>