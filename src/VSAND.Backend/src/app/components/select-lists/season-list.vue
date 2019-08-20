<template>
    <input-select v-bind:input-id="inputId"
                  label="Season"
                  choose-prompt="Select a Season"
                  v-bind:default-value="selected"
                  v-bind:required="required"
                  v-bind:external-invalid="externalInvalid"
                  v-bind:options="options"
                  v-on:change="onSelected"></input-select>
</template>

<script>
    import InputSelect from "../base/input-select/input-select.vue";

    export default {
        name: "season-list",
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
                type: [Array, String, Number, Object],
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
                selected: this.defaultValue,
                validated: false,
                cLoading: false,
                options: [ "Fall", "Winter", "Spring", "Summer" ]
            }
        },
        methods: {
            onSelected(newval) {
                this.$emit('change', newval);
            }
        }
    };
</script>