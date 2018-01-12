require "LuaModel"

LuaRoleInfoModel = class(LuaModel)

function LuaRoleInfoModel:Ctor()
    LuaModel.Ctor(self, "LuaRoleInfoModel")
    self.intValue = 32
    self.stringValue = 'hello'
end

function LuaRoleInfoModel:CallAddFriend()
    print("call add friend in lua")
end