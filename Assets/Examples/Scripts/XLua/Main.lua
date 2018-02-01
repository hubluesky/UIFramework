
require "LuaRoleInfoModel"

ModelManager = CS.VBM.ModelManager

local roleInfoModel

function Main()
    roleInfoModel = LuaRoleInfoModel.new()
end

function ChnageRoleInfoData()
    roleInfoModel.intValue = 88
    roleInfoModel.stringValue = "lua value"
end