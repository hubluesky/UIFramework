require "LuaModel"

LuaRoleInfoModel = class(LuaModel)
registerType(LuaRoleInfoModel)

function LuaRoleInfoModel:DeclareProperties()
    self.intValue = 32
    self.stringValue = 'hello'
    self.testValue = 'test'
end

function LuaRoleInfoModel:CallAddFriend()
    print("call add friend in lua")
end