﻿using System;
using Emilia.BehaviorTree.Attributes;
using Emilia.Flow;
using Emilia.Flow.Attributes;
using Emilia.Node.Attributes;
using Emilia.Variables;
using Sirenix.OdinInspector;

namespace Emilia.Statescript
{
    [FlowNodeMenu("行为/变量运算"), Serializable]
    public class SetVariableStatescriptActionAsset : StatescriptActionAsset<SetVariableStatescriptAction>, IFlowNodeDescription
    {
        [LabelText("参数"), VariableKeySelector]
        public string leftKey;

        [LabelText("运算符")]
        public VariableCalculateOperator calculateOperator;

        [HideLabel, HorizontalGroup(20)]
        public bool useDefine = true;

        [HorizontalGroup, VariableTypeFilter(nameof(leftKey)), ShowIf(nameof(useDefine))]
        public Variable rightDefineValue = new VariableObject();

        [LabelText("参数"), VariableKeySelector, HorizontalGroup, HideIf(nameof(useDefine))]
        public string rightKey;

        public string description
        {
            get
            {
                string leftDescription = VariableKeySelectorAttribute.GetDescription(this.leftKey);
                string rightDescription = this.useDefine ? this.rightDefineValue.ToString() : VariableKeySelectorAttribute.GetDescription(this.rightKey);
                return leftDescription + VariableUtility.ToDisplayString(this.calculateOperator) + rightDescription;
            }
        }
    }

    public class SetVariableStatescriptAction : StatescriptAction<SetVariableStatescriptActionAsset>
    {
        protected override void OnExecute()
        {
            Variable leftValue = graph.variablesManage.GetThisValue(this.asset.leftKey);
            Variable rightValue = this.asset.useDefine ? this.asset.rightDefineValue : graph.variablesManage.GetThisValue(this.asset.rightKey);
            if (leftValue != null && rightValue != null)
            {
                Variable result = VariableUtility.Calculate(leftValue, rightValue, this.asset.calculateOperator);
                graph.variablesManage.SetValue(this.asset.leftKey, result);
            }
        }
    }
}