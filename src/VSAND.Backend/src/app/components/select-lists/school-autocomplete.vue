<template>
    <input-autocomplete input-id="gameSearchSchool"
                        v-bind:label="hideLabel ? '' : 'School'"
                        v-bind:value.sync="selectedSchool"
                        force-select source-key="name"
                        async-src-url="/siteapi/schools/autocomplete?k="
                        async-src-item="id"></input-autocomplete>
</template>

<script>
    import InputAutocomplete from "../base/input-autocomplete/input-autocomplete.vue";

    export default {
        name: "school-autocomplete",

        components: {
            "input-autocomplete": InputAutocomplete,
        },

        props: {
            defaultValue: {
                type: Number,
                default: null,
            },
            hideLabel: {
                type: Boolean,
                default: false,
            }
        },

        data() {
            return {
                selectedSchool: null,
            }
        },

        created() {
            var vm = this;
            // load the list from the API endpoint
            if (this.defaultValue !== null && this.defaultValue > 0) {
                $.get("/siteapi/schools/autocompleterestore?schoolid=" + this.defaultValue, function (data) {
                    vm.selectedSchool = data;
                });
            }
        },

        watch: {
            selectedSchool(newval) {
                if (newval !== null) {
                    this.$emit("update:value", newval);
                }
            },

            defaultValue(newval) {
                if (newval === null) {
                    this.selectedSchool = null;
                }
            }
        }
    }
</script>