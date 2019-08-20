<template>
    <input-autocomplete v-bind:label="label"
        v-bind:value.sync="selectedTeam"
        force-select source-key="name"
        v-bind:async-src-url="'/siteapi/teams/autocomplete?sportId=' + sportId + '&scheduleYearId=' + scheduleYearId + '&q='"
        async-src-item="id"></input-autocomplete>
</template>

<script>
    import InputAutocomplete from "../base/input-autocomplete/input-autocomplete.vue";

    export default {
        name: "team-autocomplete",

        components: {
            "input-autocomplete": InputAutocomplete,
        },

        props: {
            defaultValue: {
                type: Number,
                default: null,
            },
            sportId: {
                type: Number,
                required: true,
            },
            scheduleYearId: {
                type: Number,
                required: true,
            },
            label: {
                type: String,
                default: "Team",
            },
        },

        data: function () {
            return {
                selectedTeam: null,
            }            
        },

        created() {
            var vm = this;
            // load the list from the API endpoint
            if (this.defaultValue !== null && this.defaultValue > 0) {
                $.get("/siteapi/teams/autocompleterestore?teamid=" + this.defaultValue, function (data) {
                    vm.selectedTeam = data;
                });
            }            
        },

        watch: {
            selectedTeam(newval) {
                this.$emit("update:value", newval);
            }
        }
    }
</script>