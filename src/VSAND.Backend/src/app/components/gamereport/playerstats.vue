<template>
    <widget-wrapper icon="battery-three-quarters" v-bind:title="team.name + ' Player Stats'"
                    v-bind:padding="true">
        <template v-slot:toolbarmasterbuttons>
            <game-nav v-bind:game-report-id="gameReportId"></game-nav>
        </template>

        <template v-slot:bodymessage>
            Not sure if we need to use body message slot or not, need to review
        </template>

        <b-tabs pills>
            <b-tab v-for="cat in sport.playerStatCategories"
                   v-bind:key="'playerstatcat-' + cat.sportPlayerStatCategoryId"
                   v-bind:title="cat.name">

                <table class="table table-bordered table-striped mb-0 responsive">
                    <thead>
                        <tr>
                            <th>Player</th>
                            <th v-for="stat in cat.stats">
                                <a v-b-tooltip v-bind:title="stat.name">{{ stat.displayName }}</a>
                            </th>
                            <th>&nbsp;</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="playerId in getCatPlayers(cat.sportPlayerStatCategoryId)"
                            v-bind:class="{'table-primary': playerId === highlightPlayerId}">
                            <td data-title="Player">
                                {{ getPlayerDisplay(playerId) }} <small>{{ getPlayerJerseyNumber(playerId) }}</small>
                                <div v-if="sport.enablePlayerOfRecord && sport.playerOfRecordStatCategory === cat.sportPlayerStatCategoryId">
                                    <input-checkbox v-bind:label="playerOfRecordLabel"
                                                    v-bind:default-value="playerOfRecord === playerId"
                                                    v-on:input="setPlayerOfRecord(playerId, $event)"></input-checkbox>
                                    <div v-if="playerOfRecord === playerId" class="form-inline input-number">
                                        <input-field label="W:" v-bind:input-id="'playerOfRecordWins-' + playerId"
                                                     input-type="number"
                                                     v-bind:default-value="getPlayerOfRecordWins(playerId)"
                                                     v-on:focus="highlightPlayerId = playerId"
                                                     v-on:change="setPlayerOfRecordWins(playerId, $event)">
                                        </input-field>
                                        <input-field label="L:" v-bind:input-id="'playerOfRecordLosses-' + playerId"
                                                     input-type="number"
                                                     v-bind:default-value="getPlayerOfRecordLosses(playerId)"
                                                     v-on:focus="highlightPlayerId = playerId"
                                                     v-on:change="setPlayerOfRecordLosses(playerId, $event)">
                                        </input-field>
                                        <input-field label="T:" v-bind:input-id="'playerOfRecordTies-' + playerId"
                                                     input-type="number"
                                                     v-bind:default-value="getPlayerOfRecordTies(playerId)"
                                                     v-on:focus="highlightPlayerId = playerId"
                                                     v-on:change="setPlayerOfRecordTies(playerId, $event)"
                                                     v-if="sport.allowTie">
                                        </input-field>
                                    </div>
                                </div>
                            </td>
                            <td v-for="stat in cat.stats" v-bind:data-title="stat.displayName">
                                <div class="form-inline input-number">
                                    <input-field v-bind:input-id="'playerstat' + stat.sportPlayerStatId + '-' + playerId"
                                                 input-type="number"
                                                 v-bind:default-value="getPlayerStatValue(playerId, stat.sportPlayerStatId)"
                                                 v-on:focus="highlightPlayerId = playerId"
                                                 v-on:change="setPlayerStatValue(playerId, stat.sportPlayerStatId, $event)">
                                        <template slot="inputappend">
                                            <audit-button v-if="adminUser"
                                                          audit-table="vsand_GameReportPlayerStat"
                                                          v-bind:audit-id="getPlayerStatId(playerId, stat.sportPlayerStatId)"></audit-button>
                                        </template>
                                    </input-field>
                                </div>
                            </td>
                            <td data-title="Remove">
                                <button class="btn btn-sm btn-default"><i class="far fa-trash"></i></button>
                            </td>
                        </tr>
                        <!-- This is the autocomplete insert row -->
                        <tr>
                            <td v-bind:colspan="cat.stats.length + 2">
                                <autocomplete v-bind:source="team.rosterEntries"
                                              results-display="displayName"
                                              v-bind:ref="'autocomplete-' + cat.sportPlayerStatCategoryId + '-' + team.gameReportTeamId"
                                              results-value="playerId"
                                              input-class="form-control"
                                              placeholder="Search for Player"
                                              clear-button-icon="fas fa-times"
                                              v-on:selected="addSelectedPlayer(cat.sportPlayerStatCategoryId, $event.selectedObject)"
                                              v-on:noresultsclick="onNoResultsClick($event)">
                                    <template slot="noResults">
                                        <i class="far fa-plus-circle"></i> Create a new player
                                    </template>
                                </autocomplete>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </b-tab>
        </b-tabs>

        <b-modal id="modal-1" title="Create a new player" v-model="showAddPlayer">

        </b-modal>
    </widget-wrapper>
</template>

<script>
    import { BTabs, BTab, BButton, VBTooltip, BModal } from 'bootstrap-vue';
    import WidgetWrapper from "../base/widget-wrapper/widget-wrapper.vue";
    import InputField from "../base/input-field/input-field.vue";
    import AuditButton from "../base/audit-button.vue";
    import GameNav from "./nav.vue";
    import Autocomplete from "../base/autocomplete/autocomplete.vue";
    import InputCheckbox from "../base/input-checkbox/input-checkbox.vue";

    Vue.directive('b-tooltip', VBTooltip);

    export default {
        name: "playerstats",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },

        components: {
            "b-tabs": BTabs,
            "b-tab": BTab,
            "b-modal": BModal,
            "widget-wrapper": WidgetWrapper,
            "input-field": InputField,
            "input-checkbox": InputCheckbox,
            "audit-button": AuditButton,
            "game-nav": GameNav,
            "autocomplete": Autocomplete,
        },

        props: {
            gameReportId: {
                type: Number,
                required: true,
            },
            gameReportTeam: {
                type: Object,
                required: true,
            },
            gameReportSport: {
                type: Object,
                required: true,
            },
            isAdmin: {
                type: Boolean,
                default: false,
            }
        },

        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                team: this.gameReportTeam,
                sport: this.gameReportSport,
                playersByCat: [],
                adminUser: this.isAdmin,
                highlightPlayerId: 0,
                showAddPlayer: false,
            }
        },

        computed: {
            // Computed properties
            playerOfRecordLabel() {
                let ret = "Player of Record";
                let recLabel = this.sport.playerOfRecordLabel;
                if (recLabel === null || recLabel === "") {
                    // try to build it from the indicated position
                    let recPositionId = this.sport.playerOfRecordPosition;
                    if (recPositionId !== null && recPositionId > 0) {
                        let recPosition = this.sport.positions.find(p => p.sportPositionId === recPositionId);
                        if (recPosition !== undefined && recPosition !== null) {
                            ret = recPosition.name + " of Record";
                        }
                    }
                } else {
                    ret = recLabel;
                }
                return ret;
            },

            playerOfRecord() {
                let ret = 0;
                let playerRe = this.team.gameRosterEntries.find(re => re.playerOfRecord === true);
                if (playerRe !== undefined && playerRe !== null) {
                    ret = playerRe.playerId;
                }
                return ret;
            }
        },

        created() {
            // Load whatever we need to get via ajax (not included in the Model)
            // Setup any non-reactive properties here
            // load the sport + game meta configuration
            var vm = this;

            // load the players with active stats into a list by stat category so that it can be pushed to in order to init new players in cat
            vm.sport.playerStatCategories.forEach(c => {
                // get the players that should be in here
                let statIds = [...new Set(c.stats.map(s => s.sportPlayerStatId))];
                let players = [];
                if (vm.team.playerStats !== undefined && vm.team.playerStats !== null) {
                    if (this.sport.countZeroValueStats) {
                        players = [...new Set(vm.team.playerStats.filter(ps => statIds.includes(ps.statId)).map(ps => ps.playerId))]
                    } else {
                        players = [...new Set(vm.team.playerStats.filter(ps => statIds.includes(ps.statId) && ps.statValue !== null && ps.statValue > 0).map(ps => ps.playerId))]
                    }
                }

                vm.playersByCat.push({
                    catId: c.sportPlayerStatCategoryId,
                    players: players
                });
            });
        },

        watch: {
            gameReportTeam(newVal, oldVal) {
                this.team = newVal;
            },
            gameReportSport(newVal, oldVal) {
                this.sport = newVal;
            },
            isAdmin(newVal, oldVal) {
                this.adminUser = newVal;
            },
        },

        methods: {
            // Do stuff!
            getCatPlayers(catId) {
                let selCat = this.playersByCat.find(pbc => pbc.catId === catId);
                return selCat.players;
            },

            addSelectedPlayer(catId, selectedPlayer) {
                // Clear the autocomplete input
                let curAutocomplete = this.$refs['autocomplete-' + catId + '-' + this.team.gameReportTeamId];
                console.log("curAutocomplete", curAutocomplete);
                curAutocomplete[0].clear();

                var vm = this;

                // Couple of things to take care of here
                // 1. If the selected player is already activated in the selected category list, highlight the row and abort
                let targetPlayerId = selectedPlayer.playerId;
                let curCat = this.getCatPlayers(catId);
                let foundPlayerInCat = curCat.find(p => p === targetPlayerId);
                if (foundPlayerInCat !== undefined && foundPlayerInCat !== null) {
                    // we found the player already exists in the list
                    console.log("player already in list for cat", targetPlayerId);
                    this.highlightPlayerId = targetPlayerId;
                    return;
                }

                //2. If we are adding the player, make sure that they are initialized with the stats in this category / also active in the game roster
                console.log("Add player to cat", catId, selectedPlayer);

                $.ajax({
                    method: "GET",
                    url: "/siteapi/games/initplayer/?gameid=" + vm.gameReportId + "&teamid=" + vm.gameReportTeam.gameReportTeamId + "&playerid=" + targetPlayerId,
                })
                    .done(function (data) {
                        if (data.success) {
                            // player was initialized
                            // return object has all existing stats in it
                            console.log(data);

                            let selCat = vm.playersByCat.find(c => c.catId === catId);
                            selCat.players.push(targetPlayerId);

                            //TODO: push any stats from the result back into the stats array (when it does't already exist)
                            if (data.obj !== undefined && data.obj !== null && data.obj.length > 0) {

                            }
                        } else {
                            self.$bvToast.toast(data.message, {
                                title: 'An Error Occurred',
                                appendToast: true,
                                solid: true,
                                noAutoHide: true,
                                variant: "danger"
                            });
                        }
                    }, 'json');
            },

            getPlayerDisplay(playerId) {
                let name = "Unnamed";
                // Look up the player name in the team's roster entries
                let roster = this.gameReportTeam.rosterEntries.find(re => re.playerId === playerId);
                if (roster !== undefined && roster !== null) {
                    name = roster.displayName;
                }
                return name;
            },

            getPlayerJerseyNumber(playerId) {
                let jersey = "#";
                // Look up the player name in the team's roster entries
                let roster = this.gameReportTeam.rosterEntries.find(re => re.playerId === playerId);
                if (roster !== undefined && roster !== null) {
                    jersey = "#" + roster.jerseyNumber;
                }
                return jersey;
            },

            getPlayerStatId(playerId, statId) {
                let id = 0;
                let statObj = this.gameReportTeam.playerStats.find(ps => ps.playerId === playerId && ps.statId === statId);
                if (statObj !== undefined && statObj !== null) {
                    id = statObj.playerStatId;
                }
                return id;
            },

            getPlayerStatValue(playerId, statId) {
                let value = "";
                let statObj = this.gameReportTeam.playerStats.find(ps => ps.playerId === playerId && ps.statId === statId);
                if (statObj !== undefined && statObj !== null) {
                    value = statObj.statValue;
                }
                return value;
            },

            setPlayerStatValue(playerId, statId, value) {
                var vm = this;
                let statRequest = {
                    PlayerStatId: 0,
                    GameReportId: vm.gameReportId,
                    PlayerId: playerId,
                    StatId: statId,
                    StatValue: Number(value)
                }

                $.ajax({
                    method: "POST",
                    url: "/siteapi/games/saveplayerstat",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(statRequest),
                    dataType: 'json'
                })
                    .done(function (data) {
                        if (data.success) {
                            // stat was saved, nothing to do here
                        } else {
                            self.$bvToast.toast(data.message, {
                                title: 'An Error Occurred',
                                appendToast: true,
                                solid: true,
                                noAutoHide: true,
                                variant: "danger"
                            });
                        }
                    }, 'json');
            },

            setPlayerOfRecord(playerId, val) {
                console.log("setPlayerOfRecord", playerId, val);
            },

            getPlayerOfRecordWins(playerId) {
                let ret = 0;
                let gre = this.team.gameRosterEntries.find(gre => gre.playerId === playerId);
                if (gre !== undefined && gre !== null) {
                    ret = gre.recordWins;
                }
                return ret;
            },

            getPlayerOfRecordLosses(playerId) {
                let ret = 0;
                let gre = this.team.gameRosterEntries.find(gre => gre.playerId === playerId);
                if (gre !== undefined && gre !== null) {
                    ret = gre.recordLosses;
                }
                return ret;
            },

            getPlayerOfRecordTies(playerId) {
                let ret = 0;
                let gre = this.team.gameRosterEntries.find(gre => gre.playerId === playerId);
                if (gre !== undefined && gre !== null) {
                    ret = gre.recordTies;
                }
                return ret;
            },

            setPlayerOfRecordWins(playerId, value) {

            },

            setPlayerOfRecordLosses(playerId, value) {

            },

            setPlayerOfRecordTies(playerId, value) {

            },

            onNoResultsClick(val) {
                console.log("onNoResultsClick: ", val);
                this.showAddPlayer = true;

            },
        },
    };
</script>