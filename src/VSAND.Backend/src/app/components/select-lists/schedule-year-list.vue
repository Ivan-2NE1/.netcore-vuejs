<template>
    <input-select v-bind:input-id="inputId"
                  label="Schedule Year"
                  choose-prompt="Select a Schedule Year"
                  v-bind:default-value="selected"
                  v-bind:required="required"
                  v-bind:external-invalid="externalInvalid"
                  v-bind:option-adapter="optionAdapter"
                  v-bind:options="options"
                  v-on:change="selected = $event"></input-select>
</template>

<script>
    import InputSelect from "../base/input-select/input-select.vue";

    export default {
        name: "schedule-year-list",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        components: {
            "input-select": InputSelect
        },
        props: {
            inputId: {
                type: String,
                default: "",
            },
            defaultValue: {
                type: Number,
                default: null
            },
            required: {
                type: Boolean,
                default: false
            },
            externalInvalid: {
                type: Boolean,
                default: null
            },
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                options: [],
                selected: this.defaultValue,
                validated: false,
                cLoading: false,
            }
        },
        computed: {

        },

        created() {
            var vm = this;
            // load the list from the API endpoint
            $.get("/siteapi/scheduleyears/list", function (data) {
                vm.options = data;
            });
        },

        watch: {
            selected(newval) {
                this.$emit('change', newval);
            }
        },

        methods: {
            optionAdapter(value) {
                return {
                    id: value.id,
                    label: value.name,
                    key: value.id,
                    value: value.id
                };
            }
        }
    };
</script>