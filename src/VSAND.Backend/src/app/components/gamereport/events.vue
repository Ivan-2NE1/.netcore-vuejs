<template>
    <div>
        <div v-for="event in gameReport.events" class="mb-3">
            <h3>{{ event.eventName }}</h3>

            <template v-for="result in event.results">
                <component v-bind:is="event.resultHandler"
                           v-bind:game-event-result="result"
                           v-bind:roster-entries="rosterEntries"
                           v-bind:result-types="gameReport.sport.eventResultTypes"
                           v-bind:sport-event-stats="sportEventStats(event.sportEventId)"></component>
            </template>
        </div>
    </div>
</template>

<script>
    import EventResultGolfMatchPlay from "./event-result-golf-match-play.vue";
    import EventResultIndividual from "./event-result-individual.vue";
    import EventResultRelay from "./event-result-relay.vue";
    import EventResultTennisDoubles from "./event-result-tennis-doubles.vue";
    import EventResultTennisSingles from "./event-result-tennis-singles.vue";
    import EventResultWrestling from "./event-result-wrestling.vue";

    export default {
        name: "game-events",
        components: {
            "event-result-golf-match-play": EventResultGolfMatchPlay,
            "event-result-individual": EventResultIndividual,
            "event-result-relay": EventResultRelay,
            "event-result-tennis-doubles": EventResultTennisDoubles,
            "event-result-tennis-singles": EventResultTennisSingles,
            "event-result-wrestling": EventResultWrestling,
        },
        props: {
            gameReport: {
                type: Object,
                required: true,
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.

            }
        },

        computed: {
            /**
             * The combined rosters for both teams, formatted so that last name, first name + school name is their designation in the list
            */
            rosterEntries() {
                let roster = [];
                this.gameReport.teams.forEach(t => {
                    if (t.rosterEntries !== undefined && t.rosterEntries !== null) {
                        t.rosterEntries.forEach(re => {
                            let name = re.displayName;
                            if (this.gameReport.sport.enableJerseyNumber && re.jerseyNumber !== null && re.jerseyNumber !== "") {
                                name += " #" + re.jerseyNumber;
                            }
                            name += " (" + t.name + ")";
                            roster.push({
                                playerId: re.playerId,
                                display: name,
                                gameReportTeamId: t.gameReportTeamId,
                                teamName: t.name,
                                lastName: re.lastName,
                                firstName: re.firstName,
                            })
                        });
                    }
                });

                // need to sort roster by lastname, then firstname
                roster.sort(
                    function (a, b) {
                        if (a.lastName === b.lastName) {
                            // compare first name
                            if (a.firstName === b.firstName) {
                                return a.teamName.localeCompare(b.teamName);
                            }
                            return a.firstName.localeCompare(b.firstName);
                        }
                        return a.lastName.localeCompare(b.lastName);
                    });

                return roster;
            },
        },

        created() {
            var vm = this;

        },

        methods: {
            getEventName(sportEventId) {
                let name = "Unnamed Event";
                let ev = this.gameReport.sport.events.find(e => e.sportEventId === sportEventId);
                if (ev !== undefined && ev !== null) {
                    name = ev.name;
                }
                return name;
            },

            sportEventStats(sportEventId) {
                let sportEvent = this.gameReport.sport.events.find(se => se.sportEventId === sportEventId);
                if (sportEvent !== undefined && sportEvent !== null) {
                    return sportEvent.stats;
                }
                return [];
            },

            getEventResultHandler(sportEventId) {
                let rtype = "";
                let handler = "";
                let ev = this.gameReport.sport.events.find(e => e.sportEventId === sportEventId);
                if (ev !== undefined && ev !== null) {
                    rtype = ev.resultType;
                    handler = ev.resultHandler;
                }
                if (rtype.toLowerCase() !== "custom") {
                    switch (rtype.toLowerCase()) {
                        case "individual":
                            handler = "event-result-individual";
                            break;
                        case "4player":
                            handler = "event-result-relay";
                            break;
                        default:
                            handler = "event-result-individual";
                            break;
                    }
                } else {
                    switch (handler.toLowerCase()) {
                        case "~/vsand/uc/sportevent/vsandtennissingles.ascx":
                            handler = "event-result-tennis-singles";
                            break;
                        case "~/vsand/uc/sportevent/vsandtennisdoubles.ascx":
                            handler = "event-result-tennis-doubles";
                            break;
                        case "~/vsand/uc/sportevent/vsandgolfmatchplay.ascx":
                            handler = "event-result-golf-match-play";
                            break;
                        case "~/vsand/uc/sportevent/vsandwrestlingbout.ascx":
                            handler = "event-result-wrestling";
                            break;
                        default:
                            // grrr... why is this not set?
                            handler = "event-result-individual";
                            break;
                    }
                }

                return handler;
            },
        }
    };
</script>