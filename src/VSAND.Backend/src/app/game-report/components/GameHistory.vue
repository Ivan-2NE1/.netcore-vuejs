<template>
    <widget-wrapper icon="flag-checkered" title="Report History"
                    v-bind:padding="false">

        <template v-slot:bodymessage>
            <div class="alert bg-info-600">
                <div class="d-flex align-items-center">
                    <div class="alert-icon width-8">
                        <span class="icon-stack icon-stack-xl">
                            <i class="base-2 icon-stack-3x color-danger-400"></i>
                            <i class="base-10 text-white icon-stack-1x"></i>
                            <i class="ni md-profile color-danger-800 icon-stack-2x"></i>
                        </span>
                    </div>
                    <div class="flex-1 pl-1">
                        <span class="h2">
                            Did you know...
                        </span>
                        <p>You can open game reports that were previously submitted (even by another coach) and make changes to them directly.</p>
                        <p>If you do not see your game, use the "Create New Game Report" button at the bottom of this page.</p>
                    </div>
                </div>
            </div>
        </template>

        <table class="table">
            <thead>
                <tr>
                    <th>Game Date</th>
                    <th>&nbsp;</th>
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="game in games">
                    <td>{{ game.gameDate | formatDate }}</td>
                    <td>{{ game.name }}</td>
                    <td><button class="btn btn-default" v-on:click="gotGame(game.gameReportId)">Open Game Report</button></td>
                </tr>
            </tbody>
        </table>

        <template v-slot:footer>
            <router-link to="chooseEventType" tag="button" class="btn btn-primary btn-lg">
                Create a New Game
            </router-link>
        </template>
    </widget-wrapper>
</template>

<script>
    import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";

    export default {
        name: "game-history",

        components: {
            "widget-wrapper": WidgetWrapper,
        },

        computed: {
            teamId() {
                return this.$store.getters.teamId;
            },
        },

        data: function () {
            return {
                games: [],
            }
        },

        created: function () {
            // load the games for the selected team
            var vm = this;
            // load the list from the API endpoint
            $.get("/siteapi/games/teamgames?teamid=" + vm.teamId, function (data) {
                vm.games = data;
            });
        },

        methods: {
            gotGame(gameReportId) {
                window.location = "/game/" + gameReportId;
            }
        },

        mounted() {
            var vm = this;
            document.addEventListener('keydown', function (event) {
                if (event.code == "Enter") {
                    var paths = window.location.href.split("/");
                    var lastPath = paths[paths.length - 1];
                    if (lastPath != "history") {
                        return;
                    }
                    vm.$router.push({ name: 'chooseEventType', params: {} });
                }
            });
        }
    }

</script>