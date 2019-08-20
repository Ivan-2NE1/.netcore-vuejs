<template>
    <div class="form-group">
        <div class="form-inline">
            <input-select choose-prompt="Choose"
                          v-bind:default-value="selected"
                          v-bind:required="required"
                          v-bind:external-invalid="externalInvalid"
                          v-bind:option-adapter="optionAdapter"
                          v-bind:options="options"
                          v-on:change="onSelected"></input-select>
            <input-field v-bind:default-value="selected"
                         v-bind:max-length="4"
                         v-on:input="onInput"></input-field>
        </div>

    </div>

</template>

<script>
    import InputSelect from "../base/input-select/input-select.vue";
    import InputField from "../base/input-field/input-field.vue";

    export default {
        name: "graduationyear-list",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        components: {
            "input-select": InputSelect,
            "input-field": InputField
        },
        props: {
            defaultValue: {
                type: [Number, String],
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
                options: [],
            }
        },
        mounted() {
            // build our options list
            var iCurMonth = new Date().getMonth() + 1;
            var iCurYear = new Date().getFullYear();
            var iCurClass = (iCurMonth > 6) ? iCurYear + 1 : iCurYear;

            var oOpts = [];
            oOpts.push({ "key": "sr", "value": iCurClass, "label": "Senior" });
            oOpts.push({ "key": "jr", "value": iCurClass + 1, "label": "Junior" });
            oOpts.push({ "key": "soph", "value": iCurClass + 2, "label": "Sophomore" });
            oOpts.push({ "key": "frosh", "value": iCurClass + 3, "label": "Freshman" });
            oOpts.push({ "key": "pg", "value": iCurClass, "label": "Post-Grad" });
            oOpts.push({ "key": "unk", "value": 0, "label": "Unknown" });
            this.options = oOpts;
        },
        methods: {
            onSelected(newval) {                
                this.onInput(newval.value);
            },

            onInput(newval) {
                this.selected = newval;
                this.$emit("change", newval);
            },

            optionAdapter(value) {
                return {
                    label: value.label,
                    key: value.key,
                    value: value.value.toString()
                };
            }
        },
        watch: {
            defaultValue(newval) {
                this.selected = newval;
            }
        }
    };
</script>