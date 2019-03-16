#查找某个目录下的目标文件
import os       #引入操作系统模块
import sys      #用于标准输入输出
import re

#搜索Mainfest文件
p1=re.compile("/Users/rdm/ieg_ci/slave/workspace/dev_android_assetbundles_dev/Naruto/AssetBundles/android/icon/")
def search_one_manifest_file(mainfest_file):
    fileObj=open(mainfest_file)
    for line in fileObj:
        if p1.search(line):
            print(mainfest_file)

#搜索   
def search_mainfest_file_in_working_dir():
    for (root, dirs, files) in os.walk(os.getcwd()):
        for filename in files:
            filepath = os.path.join(root,filename)
            #print(filepath)
            if filepath.endswith(".manifest"):
                search_one_manifest_file(filepath) 
                 
#Main              
if __name__ == "__main__":
    print('[Main][Begin]')
    search_mainfest_file_in_working_dir()
    print('[Main][End]')