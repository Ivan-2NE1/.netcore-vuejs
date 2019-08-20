<template>
    <div class="form-group">
        <div v-bind:class="['custom-control custom-checkbox', {'has-error': !valid}]">
            <input class="custom-control-input"
                   type="checkbox"
                   v-bind:id="inputId"
                   v-bind:checked="inputValue"
                   v-bind:disabled="isDisabled"
                   v-on:change="$emit('input', $event.target.checked)" />
            <label class="custom-control-label" v-bind:for="inputId === '' ? false : inputId">
                {{ label }}
            </label>
            <small class="form-text text-muted" v-if="formHelp !== null && formHelp !== ''">{{ formHelp }}</small>
        </div>
    </div>
</template>

<script>
    export default {
        name: "input-checkbox",
        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-checkbox-' + Math.random().toString(36).substr(2, 9)
                }
            },
            defaultValue: {
                type: Boolean,
                default: false
            },
            disabled: {
                type: Boolean,
                required: false,
                default: false
            },
            label: {
                type: String,
                default: ""
            },
            required: {
                type: Boolean,
                default: false
            },
            externalInvalid: {
                type: Boolean,
                default: null
            },
            formHelp: {
                type: String,
                default: ""
            }
        },
        data() {
            return {
                inputValue: this.defaultValue,
                validated: false,
                isDisabled: this.disabled,
            };
        },
        computed: {
            valid() {
                if (this.externalInvalid !== null && this.externalInvalid) return false;
                if (!this.required) return true;
                if (this.required && !this.inputValue) return false;
                return true;
            }
        },
        watch: {
            defaultValue(newval, oldval) {
                this.inputValue = newval;
            },
            externalInvalid(newval, oldval) {
                this.validated = true;
            },
            disabled(newval) {
                this.isDisabled = newval;
            }
        }
    };
</script>