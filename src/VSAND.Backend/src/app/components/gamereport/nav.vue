<template>
    <div id="activeGameNav" v-html="navHtml" v-if="gameReportId > 0"></div>
</template>

<script>
    export default {
        name: "game-nav",
        props: {
            gameReportId: {
                type: Number,
                default: 0
            },
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                navHtml: "",
            }
        },

        created() {
            var vm = this;

            $.get("/game/nav/" + vm.gameReportId, function (response) {
                vm.navHtml = response;
            });
        },
    };
</script>