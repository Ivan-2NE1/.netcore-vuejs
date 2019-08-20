<template>
    <div>
        I am the golf match play result handler
    </div>
</template>

<script>
    export default {
        name: "event-result-golf-match-play",
        props: {
            gameEventResult: {
                type: Object,
                required: true,
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                eventResult: this.gameEventResult,
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

        }
    };
</script>