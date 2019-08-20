<template>
    <widget-wrapper icon="battery-three-quarters" title="Team Stats"
                    v-bind:padding="false">
        <template v-slot:toolbarmasterbuttons>
            <game-nav v-bind:game-report-id="game.gameReportId"></game-nav>
        </template>

        <table class="table table-bordered table-striped mb-0 responsive">
            <thead>
                <tr>
                    <th>&nbsp;</th>
                    <th v-for="team in sortedTeams">
                        {{ team.name }}
                    </th>
                </tr>
            </thead>
            <tbody is="team-stat-category" 
                   v-for="cat in game.sport.teamStatCategories" 
                   v-bind:category="cat" 
                   v-bind:teams="sortedTeams" 
                   v-bind:key="'teamstatcat-' + cat.sportTeamStatCategoryId"
                   v-bind:is-admin="adminUser"></tbody>
        </table>
    </widget-wrapper>
</template>

<script>
    import WidgetWrapper from "../base/widget-wrapper/widget-wrapper.vue";
    import TeamStatCategory from "./team-stat-category.vue";
    import GameNav from "./nav.vue";

    export default {
        name: "teamstats",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },

        components: {
            "widget-wrapper": WidgetWrapper,
            "team-stat-category": TeamStatCategory,
            "game-nav": GameNav,
        },

        props: {
            gameReport: {
                type: Object,
                required: true,
            },
            isAdmin: {
                type: Boolean,
                default: false,
            },
        },

        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                game: this.gameReport,
                adminUser: this.isAdmin,
            }
        },

        computed: {
            // Computed properties
            // Computed properties
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
        },

        created() {
            // Load whatever we need to get via ajax (not included in the Model)
            // Setup any non-reactive properties here
            // load the sport + game meta configuration
            var vm = this;

        },

        watch: {
            isAdmin(newVal, oldVal) {
                this.adminUser = newVal;
            },
        },

        methods: {
            // Do stuff!

        },
    };
</script>