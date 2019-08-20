<template>
    <div v-bind:class="['form-group', {'mb-0': !padding}]">
        <label v-if="label && label.length > 0"
               :class="['control-label', {'has-value': hasValue}]"
               :for="inputId === '' ? false : inputId">{{ label }}</label>
        <v-select v-model="selected"
                  v-bind:options="localOptions"
                  v-bind:label="optionLabelKey"
                  v-bind:multiple="enableMultiple"
                  v-on:input="selected = $event" />
        <div class="form-text text-muted" v-if="formHelp !== null && formHelp !== ''">{{ formHelp }}</div>
    </div>
</template>

<script>
    import vSelect from 'vue-select'

    export default {
        name: "vue-selectbase",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        components: {
            "v-select": vSelect,
        },
        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-selectbase-' + Math.random().toString(36).substr(2, 9)
                }
            },
            options: {
                type: Array,
                default: []
            },
            optionLabelKey: {
                type: String,
                default: null
            },
            enableMultiple: {
                type: Boolean,
                default: false
            },
            defaultValue: {
                type: [Array, String, Number, Object],
                default: null
            },
            label: {
                type: String,
                default: ""
            },
            formHelp: {
                type: String,
                default: "",
            },
            required: {
                type: Boolean,
                default: false
            },
            externalInvalid: {
                type: Boolean,
                default: null
            },
            choosePrompt: {
                type: String,
                default: "Choose"
            },
            padding: {
                type: Boolean,
                default: true
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                selected: this.defaultValue,
                localOptions: this.options,
                validated: false,
            };
        },
        computed: {
            valid() {
                if (!this.validated) return true;
                if (this.externalInvalid !== null && this.externalInvalid) return false;
                if (!this.required) return true;
                if (this.selected === null || this.selected === "") return false;
                return true;
            },
            hasValue() {
                return (this.selected !== null && this.selected !== "");
            },            
        },
        watch: {
            externalInvalid(newval) {
                this.validated = true;
            },
            loading(newval) {
                this.cLoading = newval;
            },
            //defaultValue(newVal) {
            //    this.selected = newVal;
            //},
            selected(newVal) {
                this.$emit("change", newVal);
            },
            options(newVal) {
                if (newVal === undefined || newVal === null) {
                    this.localOptions = [];
                } else {
                    this.localOptions = newVal;
                }                
            }
        },
        methods: {
            getDefaultValue() {
                switch (typeof this.defaultValue) {
                    case "string":
                        return "";
                        break;
                    case "number":
                        return "";
                        break;
                    case "object":
                        return null;
                        break;
                    default:
                        return null;
                        break;
                }
            },
        }
    };
</script>

<style lang="scss">
    @import "~vue-select/src/scss/vue-select.scss";

    .vs__dropdown-toggle {
        background-color:#FFF;
        border:1px solid #ccc;
        padding:0;
    }
    
    .vs__selected-options {
        padding:0;
        margin:6px 0;
    }

    .vs__selected {
        margin:0px 6px;
        padding:3px 5px 2px 5px;
    }
</style>

   